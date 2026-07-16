using System.Reflection;
using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Entites.Resume;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.DbContext.AppDbContext;

public class JobFlowDbContext : IdentityDbContext<AppUser,Role, Guid>
{

    public JobFlowDbContext(DbContextOptions<JobFlowDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<JobPost> JobPosts { get; set; }
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<AttachmentFile> AttachmentsFiles { get; set; }
    public DbSet<ResumeBuilder> ResumeBuilders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SavedJob> SavedJobs { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<CompanyFeature> CompanyFeatures { get; set; }
    public DbSet<NotificationLog> NotificationLogs { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}



