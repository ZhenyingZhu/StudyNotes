using System.Text.Json;
using System.Text.Json.Serialization;
using ItemOrganizer.Api;
using ItemOrganizer.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ITEMORGANIZER_DATABASE_CONNECTION"];
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "ITEMORGANIZER_DATABASE_CONNECTION is required.");
}

builder.Services.AddDbContext<ItemOrganizerDbContext>(options =>
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
builder.Services.AddProblemDetails();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_1;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CurrentUserAccessor>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>()
            ?? ["http://localhost:5173"];
        policy.WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddSingleton<
    Microsoft.AspNetCore.Authorization.IAuthorizationMiddlewareResultHandler,
    ProblemDetailsAuthorizationResultHandler>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.Audience = builder.Configuration["Authentication:Audience"];
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.Read, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(
            new ScopeAccessRequirement(AuthorizationPolicies.Read));
    });
    options.AddPolicy(AuthorizationPolicies.Write, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(
            new ScopeAccessRequirement(AuthorizationPolicies.Write));
    });
});
builder.Services.AddSingleton<
    Microsoft.AspNetCore.Authorization.IAuthorizationHandler,
    ScopeAccessHandler>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapReadEndpoints();
app.MapWriteEndpoints();

app.Run();

public partial class Program;
