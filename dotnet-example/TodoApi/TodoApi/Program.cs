using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoApi.Models;
using Microsoft.AspNetCore.Identity;
using TodoApi.Data;
using TodoApi.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TodoApiContextConnection") ?? throw new InvalidOperationException("Connection string 'TodoApiContextConnection' not found.");

builder.Services.AddDbContext<TodoApiContext>(options =>
    options.UseSqlServer(connectionString));;

builder.Services.AddDefaultIdentity<TodoApiUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<TodoApiContext>();;
// var connectionString = builder.Configuration.GetConnectionString("TodoContextConnection");

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(connectionString));

// Add the repository.
builder.Services.AddScoped<TodoRepository>();

// Add services to the container.
builder.Services.AddControllers();

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

app.UseHttpsRedirection();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllers();

app.Run();