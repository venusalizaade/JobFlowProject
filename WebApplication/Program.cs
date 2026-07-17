
using JobFlowProject.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Business.Interfaces.JobPost;
using JobFlowProject.Business.Interfaces.Log;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Business.Services;
using JobFlowProject.Business.Services.Authentication;
using JobFlowProject.Business.Services.CompaneisService;
using JobFlowProject.Business.Services.job;
using JobFlowProject.Business.Services.Log;
using JobFlowProject.Business.Services.User;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DataSeeder;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JobFlowProject.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Middleware;
using IJobApplicationService = JobFlowProject.Business.Interfaces.EmployerInterfaces.IJobApplicationService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddDbContext<JobFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<AppUser, Role>()
    .AddEntityFrameworkStores<JobFlowDbContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Audience))
    };
});


builder.Services.AddScoped<GlobalExceptionHandlerMiddleware>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IJobPostService, JobPostService>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IJobPostRepository, JobPostRepository>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>(); 


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<JobFlowDbContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();

        // اجرای مهاجرت‌ها (تا دیتابیس ساخته شود)
        await context.Database.MigrateAsync(); 
        
        // اجرای سیدینگ
        await DataSeeder.SeedAsync(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        // اگر موقع سید کردن به مشکل خوردیم، اینجا لاگ می‌اندازیم
        Console.WriteLine($"خطا در سید کردن دیتابیس: {ex.Message}");
    }
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();


