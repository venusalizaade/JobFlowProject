using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Business.Interfaces.JobPost;
using JobFlowProject.Business.Interfaces.Log;
using JobFlowProject.Business.Interfaces.Review;
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
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JobFlowProject.Infrastructure.DataSeeder;
using JobFlowProject.Infrastructure.Repositories;
using JobFlowProject.Infrastructure.Repositories.User;
using JovFlowProject.JobMvc.Models.Binders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute());
    options.ModelBinderProviders.Insert(0, new PersianDigitsModelBinderProvider());
});

builder.Services.AddDbContext<JobFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.CommandTimeout(120)));

builder.Services
    .AddIdentity<AppUser, Role>(options =>
    {
        options.ClaimsIdentity.UserNameClaimType = System.Security.Claims.ClaimTypes.Name;
        options.ClaimsIdentity.UserIdClaimType = System.Security.Claims.ClaimTypes.NameIdentifier;
        options.ClaimsIdentity.RoleClaimType = System.Security.Claims.ClaimTypes.Role;
    })
    .AddEntityFrameworkStores<JobFlowDbContext>()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<JovFlowProject.JobMvc.Services.AppUserClaimsPrincipalFactory>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("JobSeeker", policy => policy.RequireRole(RoleConstants.JobSeekerRoleName));
    options.AddPolicy("Admin", policy => policy.RequireRole(RoleConstants.AdminRoleName));
    options.AddPolicy("ApprovedEmployer", policy =>
    {
        policy.RequireRole(RoleConstants.EmployerRoleName);
        policy.RequireClaim("IsApproved", "true");
    });
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IJobPostRepository, JobPostRepository>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<ICompanyFeatureRepository, CompanyFeatureRepository>();
builder.Services.AddScoped<IJobFeatureRepository, JobFeatureRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<ISavedJobRepository, SavedJobRepository>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IJobPostService, JobPostService>();
builder.Services.AddScoped<IJobApplicationService, JobApplicationService>();
builder.Services.AddScoped<IJobSeekerService, JobSeekerService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<ICompanyFeatureService, CompanyFeatureService>();
builder.Services.AddScoped<IJobFeatureService, JobFeatureService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IFeatureExpiryService, FeatureExpiryService>();
builder.Services.AddScoped<ISavedJobService, SavedJobService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHostedService<JovFlowProject.JobMvc.Services.FeatureExpiryHostedService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<JobFlowDbContext>();
    db.Database.Migrate();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    await DataSeeder.SeedAsync(db, userManager, roleManager);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();