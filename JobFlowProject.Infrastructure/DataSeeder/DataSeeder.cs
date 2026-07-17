using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Entities.Componies;

namespace JobFlowProject.Infrastructure.DataSeeder;

using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Entities.Job; 
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.AspNetCore.Identity;

public static class DataSeeder
{
    public static async Task SeedAsync(JobFlowDbContext context, UserManager<AppUser> userManager, RoleManager<Role> roleManager)
    {

        // ۲. ایجاد ادمین و کاربران
        var admin = new AppUser { UserName = "admin@site.com", Email = "admin@site.com" };
        await userManager.CreateAsync(admin, "Admin@123");
        await userManager.AddToRoleAsync(admin, "Admin");

        var employer1 = new AppUser { UserName = "emp1@site.com", Email = "emp1@site.com" };
        await userManager.CreateAsync(employer1, "Emp@123");
        await userManager.AddToRoleAsync(employer1, "Employer");

        var seeker1 = new AppUser { UserName = "seeker1@site.com", Email = "seeker1@site.com" };
        await userManager.CreateAsync(seeker1, "Seek@123");
        await userManager.AddToRoleAsync(seeker1, "JobSeeker");

        // ۳. ایجاد کتگوری‌ها (۱۰ تا)
        var categories = new List<Category> {
            new() { Id = Guid.NewGuid(), Name = "برنامه‌نویسی" },
            new() { Id = Guid.NewGuid(), Name = "طراحی گرافیک" },
            new() { Id = Guid.NewGuid(), Name = "دیجیتال مارکتینگ" },
            new() { Id = Guid.NewGuid(), Name = "حسابداری" },
            new() { Id = Guid.NewGuid(), Name = "مدیریت پروژه" },
            new() { Id = Guid.NewGuid(), Name = "منابع انسانی" },
            new() { Id = Guid.NewGuid(), Name = "فروش" },
            new() { Id = Guid.NewGuid(), Name = "آموزش" },
            new() { Id = Guid.NewGuid(), Name = "خدمات مشتریان" },
            new() { Id = Guid.NewGuid(), Name = "مهندسی مکانیک" }
        };
        await context.Categories.AddRangeAsync(categories);

        var tehran = new City { Id = Guid.NewGuid(), Name = "تهران" };
        var isfahan = new City { Id = Guid.NewGuid(), Name = "اصفهان" };

        await context.Cities.AddRangeAsync(tehran, isfahan);
        await context.SaveChangesAsync();


        var companies = new List<Company> {
            new() { 
                Id = Guid.NewGuid(), 
                Name = "شرکت فناوران", 
                CityId = tehran.Id,   
                City = tehran         
            },
            new() { 
                Id = Guid.NewGuid(), 
                Name = "توسعه‌گران پارس", 
                CityId = isfahan.Id, 
                City = isfahan 
            }
        };
        await context.Companies.AddRangeAsync(companies);
        await context.SaveChangesAsync(); 

        // ۵. جاب پست‌ها
        var jobs = new List<JobPost> {
            new() { Id = Guid.NewGuid(), Title = "برنامه‌نویس ارشد",  CompanyId = companies[0].Id, CategoryId = categories[0].Id },
            new() { Id = Guid.NewGuid(), Title = "گرافیست", CompanyId = companies[0].Id, CategoryId = categories[1].Id },
            new() { Id = Guid.NewGuid(), Title = "کارشناس فروش", CompanyId = companies[1].Id, CategoryId = categories[6].Id }
        };
        await context.JobPosts.AddRangeAsync(jobs);
        await context.SaveChangesAsync();

        // ۶. جاب اپلیکیشن‌ها
        var apps = new List<JobApplication> {
            new() { Id = Guid.NewGuid(), JobPostId = jobs[0].Id},
            new() { Id = Guid.NewGuid(), JobPostId = jobs[2].Id}, 
        };
        await context.JobApplications.AddRangeAsync(apps);
        await context.SaveChangesAsync();
    }
}