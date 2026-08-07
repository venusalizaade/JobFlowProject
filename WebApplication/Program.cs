using System.Text;
using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Business.Interfaces.JobPost;
using JobFlowProject.Business.Interfaces.Log;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Business.Services;
using JobFlowProject.Business.Services.Authentication;
using JobFlowProject.Business.Services.CompaneisService;
using JobFlowProject.Business.Services.EmailSender;
using JobFlowProject.Business.Services.job;
using JobFlowProject.Business.Services.Job;
using JobFlowProject.Business.Services.Log;
using JobFlowProject.Business.Services.MailKit;
using JobFlowProject.Business.Services.User;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Domain.Interfaces.Repository.User;
using JobFlowProject.Infrastructure.DataSeeder;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JobFlowProject.Infrastructure.Repositories;
using JobFlowProject.Infrastructure.Repositories.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplication1.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ===== Services =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<JobFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, Role>()
    .AddEntityFrameworkStores<JobFlowDbContext>()
    .AddDefaultTokenProviders();

// ===== JWT =====
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Secret))
{
    throw new Exception("JwtSettings not configured properly!");
}

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken=true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"=== JWT FAILED: {context.Exception.Message} ===");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("=== JWT VALIDATED ===");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
        Console.WriteLine(
        $"JWT challenge: {context.Error} - {context.ErrorDescription}"
        );

        return Task.CompletedTask;
    },

    OnMessageReceived = context =>
    {
        Console.WriteLine(
            $"Received token: {context.Token}"
        );

        return Task.CompletedTask;
    }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApprovedEmployer", policy =>
    {
        policy.RequireRole(RoleConstants.EmployerRoleName);
        policy.RequireClaim("IsApproved", "true");
    });

    options.AddPolicy("CanApproveEmployer", policy =>
    {
        policy.RequireClaim(ClaimConstants.CanApproveEmployer, "true");
    });
});

// ===== DI =====
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
builder.Services.AddScoped<IJobPostRepository, JobPostRepository>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IJobSeekerService, JobSeekerService>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<ICompanyFeatureRepository, CompanyFeatureRepository>();
builder.Services.AddScoped<ICompanyFeatureService, CompanyFeatureService>();
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// ===== Swagger (درست شده) =====
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JobFlow API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your valid token."
    });


    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ===== Build =====




// ===== Build =====
var app = builder.Build();

// ===== Seed Data =====
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<JobFlowDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();

    DataSeeder.SeedAsync(context, userManager, roleManager).GetAwaiter().GetResult();
}

// ===== Middleware =====
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapControllers();
app.Use(async (context, next) =>
{
    var authHeader = context.Request.Headers["Authorization"].ToString();
    Console.WriteLine($"=== Incoming Request: {context.Request.Method} {context.Request.Path} ===");
    Console.WriteLine($"=== Authorization Header: '{authHeader}' ===");
    await next();
});
app.Run();