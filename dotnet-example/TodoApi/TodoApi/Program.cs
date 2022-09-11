using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoApi.Models;
using Microsoft.AspNetCore.Identity;
using TodoApi.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TodoContextConnection") ?? throw new InvalidOperationException("Connection string 'TodoContextConnection' not found.");

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(connectionString));;

builder.Services.AddDefaultIdentity<TodoApiUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<TodoContext>();;

// Add the repository.
builder.Services.AddScoped<TodoRepository>();

// Add services to the container.
builder.Services.AddControllers();

// zhenying: need Razor engine for Identity UI.
builder.Services.AddRazorPages();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
}

// To serve index page.
app.UseDefaultFiles();
app.UseStaticFiles();

// zhenying: need routing for Identity UI.
app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

// zhenying: set the ClaimPricipal.Current
app.Use((context, next) =>
{
    Thread.CurrentPrincipal = context.User;
    return next(context);
});

app.MapRazorPages();

app.MapControllers();

app.Run();
