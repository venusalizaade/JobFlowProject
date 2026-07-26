using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Infrastructure.DataSeeder;

using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.AspNetCore.Identity;

public static class DataSeeder
{
    public static async Task SeedAsync(
        JobFlowDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<Role> roleManager)
    {
        Console.WriteLine("Seeder Started");

        if (await context.Companies.AnyAsync())
            return;
        try
        {
            // تمام کدهای Seeder


            // ---------------- Roles ----------------

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new Role("Admin"));

            if (!await roleManager.RoleExistsAsync("Employer"))
                await roleManager.CreateAsync(new Role("Employer"));

            if (!await roleManager.RoleExistsAsync("JobSeeker"))
                await roleManager.CreateAsync(new Role("JobSeeker"));
            Console.WriteLine("Roles Done");

            // ---------------- Users ----------------

            // ---------------- Admin ----------------

            var admin = await userManager.FindByNameAsync("1111111111");

            if (admin == null)
            {
                admin = new AppUser(
                    "Admin",
                    "System",
                    "1111111111",
                    "admin@site.com",
                    "09120000001",
                    "Male");

                var result = await userManager.CreateAsync(admin, "Admin@12345");

                if (!result.Succeeded)
                    throw new Exception(string.Join(" | ", result.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(admin, "Admin");
            }

            Console.WriteLine("Admin Done");

// ---------------- Employer ----------------

            var employer = await userManager.FindByNameAsync("2222222222");

            if (employer == null)
            {
                employer = new AppUser(
                    "Ali",
                    "Employer",
                    "2222222222",
                    "emp@site.com",
                    "09120000002",
                    "Male");

                employer.IsApproved = true;

                var result = await userManager.CreateAsync(employer, "Emp@12345");

                if (!result.Succeeded)
                    throw new Exception(string.Join(" | ", result.Errors.Select(e => e.Description)));

                employer.IsApproved = true;
                await userManager.UpdateAsync(employer);

                await userManager.AddToRoleAsync(employer, "Employer");
            }

            Console.WriteLine("Employer Done");

// ---------------- JobSeeker ----------------

            var seeker = await userManager.FindByNameAsync("3333333333");

            if (seeker == null)
            {
                seeker = new AppUser(
                    "Sara",
                    "JobSeeker",
                    "3333333333",
                    "seeker@site.com",
                    "09120000003",
                    "Female");

                var result = await userManager.CreateAsync(seeker, "Seek@12345");

                if (!result.Succeeded)
                    throw new Exception(string.Join(" | ", result.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(seeker, "JobSeeker");
            }

            Console.WriteLine("JobSeeker Done");

            // ---------------- Province ----------------

            var tehranProvince = new Province("تهران");
            var isfahanProvince = new Province("اصفهان");

            context.provinces.AddRange(
                tehranProvince,
                isfahanProvince);

            await context.SaveChangesAsync();

            // ---------------- City ----------------

            var tehran = new City(
                "تهران",
                tehranProvince.Id);

            var karaj = new City(
                "کرج",
                tehranProvince.Id);

            context.Cities.AddRange(
                tehran,
                karaj);

            await context.SaveChangesAsync();

            // ---------------- Category ----------------

            var programming = new Category(
                "برنامه نویسی",
                "Programming");

            var accounting = new Category(
                "حسابداری",
                "Accounting");

            context.Categories.AddRange(
                programming,
                accounting);

            await context.SaveChangesAsync();

            // ---------------- Skill ----------------

            var csharp = new Skill(
                "C#",
                programming.Id);

            var sql = new Skill(
                "SQL Server",
                programming.Id);

            context.Skills.AddRange(
                csharp,
                sql);

            await context.SaveChangesAsync();

            // ---------------- Company ----------------

            var company1 = new Company(
                "شرکت فناوران",
                "14000000001",
                employer.Id,
                tehranProvince.Id,
                tehran.Id,
                "تهران");

            var company2 = new Company(
                "شرکت پارس",
                "14000000002",
                employer.Id,
                tehranProvince.Id,
                karaj.Id,
                "کرج");

            context.Companies.AddRange(
                company1,
                company2);

            await context.SaveChangesAsync();
            // ---------------- Job Posts ----------------

            var job1 = new JobPost(
                "Senior .NET Developer",
                "Backend Development",
                tehranProvince.Id,
                tehran.Id,
                EmploymentTypeEnum.FullTime,
                50000000,
                company1.Id,
                programming.Id,
                csharp.Id);

            var job2 = new JobPost(
                "SQL Developer",
                "Database Development",
                tehranProvince.Id,
                karaj.Id,
                EmploymentTypeEnum.FullTime,
                40000000,
                company2.Id,
                programming.Id,
                sql.Id);

            context.JobPosts.AddRange(
                job1,
                job2);

            await context.SaveChangesAsync();

            // ---------------- Job Applications ----------------

            var application1 = new JobApplication(
                job1.Id,
                seeker.Id);

            var application2 = new JobApplication(
                job2.Id,
                seeker.Id);

            context.JobApplications.AddRange(
                application1,
                application2);

            await context.SaveChangesAsync();
            Console.WriteLine("Seeder Finished");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
}