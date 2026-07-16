
using JobFlowProject.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Business.Interfaces.Log;
using JobFlowProject.Business.Services;
using JobFlowProject.Business.Services.CompaneisService;
using JobFlowProject.Business.Services.Log;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JobFlowProject.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<JobFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<AppUser, Role>()
    .AddEntityFrameworkStores<JobFlowDbContext>()
    .AddDefaultTokenProviders();




builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()!;

builder.Services.AddScoped<GlobalExceptionHandlerMiddleware>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();



