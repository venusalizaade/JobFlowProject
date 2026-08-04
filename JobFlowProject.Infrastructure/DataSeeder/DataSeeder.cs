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

        try
        {
            // ---------------- Roles ----------------

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new Role("Admin"));

            if (!await roleManager.RoleExistsAsync("Employer"))
                await roleManager.CreateAsync(new Role("Employer"));

            if (!await roleManager.RoleExistsAsync("JobSeeker"))
                await roleManager.CreateAsync(new Role("JobSeeker"));

            Console.WriteLine("Roles Done");

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
            {
                    throw new Exception(string.Join(" | ", result.Errors.Select(e => e.Description)));

                await userManager.AddToRoleAsync(admin, "Admin");
                }
                else
                {
                    // اگر کاربر قبلاً ساخته شده، مطمئن شو نقش Admin را دارد!
                    var roles = await userManager.GetRolesAsync(admin);
                    if (!roles.Contains("Admin"))
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                        Console.WriteLine(">>> نقش Admin به کاربر موجود اضافه شد!");
                    }
                }
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

            var tehranProvince = await context.provinces.FirstOrDefaultAsync(p => p.Name == "تهران");
            if (tehranProvince == null)
            {
                tehranProvince = new Province("تهران");
                context.provinces.Add(tehranProvince);
                await context.SaveChangesAsync();
            }

            var isfahanProvince = await context.provinces.FirstOrDefaultAsync(p => p.Name == "اصفهان");
            if (isfahanProvince == null)
            {
                isfahanProvince = new Province("اصفهان");
                context.provinces.Add(isfahanProvince);
                await context.SaveChangesAsync();
            }

            Console.WriteLine("Provinces Done");

            // ---------------- City ----------------

            var tehran = await context.Cities.FirstOrDefaultAsync(c => c.Name == "تهران");
            if (tehran == null)
            {
                tehran = new City("تهران", tehranProvince.Id);
                context.Cities.Add(tehran);
                await context.SaveChangesAsync();
            }

            var karaj = await context.Cities.FirstOrDefaultAsync(c => c.Name == "کرج");
            if (karaj == null)
            {
                karaj = new City("کرج", tehranProvince.Id);
                context.Cities.Add(karaj);
                await context.SaveChangesAsync();
            }

            Console.WriteLine("Cities Done");

            // ---------------- Category ----------------

            var programming = await context.Categories.FirstOrDefaultAsync(c => c.Name == "برنامه نویسی");
            if (programming == null)
            {
                programming = new Category("برنامه نویسی", "Programming");
                context.Categories.Add(programming);
                await context.SaveChangesAsync();
            }

            var accounting = await context.Categories.FirstOrDefaultAsync(c => c.Name == "حسابداری");
            if (accounting == null)
            {
                accounting = new Category("حسابداری", "Accounting");
                context.Categories.Add(accounting);
                await context.SaveChangesAsync();
            }

            Console.WriteLine("Categories Done");

            // ---------------- Skill ----------------

            var csharp = await context.Skills.FirstOrDefaultAsync(s => s.Name == "C#");
            if (csharp == null)
            {
                csharp = new Skill("C#", programming.Id);
                context.Skills.Add(csharp);
                await context.SaveChangesAsync();
            }

            var sql = await context.Skills.FirstOrDefaultAsync(s => s.Name == "SQL Server");
            if (sql == null)
            {
                sql = new Skill("SQL Server", programming.Id);
                context.Skills.Add(sql);
                await context.SaveChangesAsync();
            }

            Console.WriteLine("Skills Done");

            // ---------------- Company ----------------

            var company1 = await context.Companies.FirstOrDefaultAsync(c => c.NationalId == "14000000001");
            if (company1 == null)
            {
                company1 = new Company(
                    "شرکت فناوران",
                    "14000000001",
                    employer.Id,
                    tehranProvince.Id,
                    tehran.Id,
                    "تهران");

                context.Companies.Add(company1);
                await context.SaveChangesAsync();
            }

            var company2 = await context.Companies.FirstOrDefaultAsync(c => c.NationalId == "14000000002");
            if (company2 == null)
            {
                company2 = new Company(
                    "شرکت پارس",
                    "14000000002",
                    employer.Id,
                    tehranProvince.Id,
                    karaj.Id,
                    "کرج");

                context.Companies.Add(company2);
                await context.SaveChangesAsync();
            }

            Console.WriteLine("Companies Done");

            // ---------------- Job Posts ----------------

            var job1 = await context.JobPosts.FirstOrDefaultAsync(j => j.Title == "Senior .NET Developer");
            if (job1 == null)
            {
                job1 = new JobPost(
                    "Senior .NET Developer",
                    "Backend Development",
                    tehranProvince.Id,
                    tehran.Id,
                    EmploymentTypeEnum.FullTime,
                    50000000,
                    company1.Id,
                    programming.Id,
                    csharp.Id);

                context.JobPosts.Add(job1);
                await context.SaveChangesAsync();
            }

            var job2 = await context.JobPosts.FirstOrDefaultAsync(j => j.Title == "SQL Developer");
            if (job2 == null)
            {
                job2 = new JobPost(
                    "SQL Developer",
                    "Database Development",
                    tehranProvince.Id,
                    karaj.Id,
                    EmploymentTypeEnum.FullTime,
                    40000000,
                    company2.Id,
                    programming.Id,
                    sql.Id);

                context.JobPosts.Add(job2);
                await context.SaveChangesAsync();
            }

            Console.WriteLine("JobPosts Done");

            // ---------------- Job Applications ----------------

            var application1Exists = await context.JobApplications
                .AnyAsync(a => a.JobPostId == job1.Id && a.JobSeekerId == seeker.Id);

            if (!application1Exists)
            {
                var application1 = new JobApplication(job1.Id, seeker.Id);
                context.JobApplications.Add(application1);
            }

            var application2Exists = await context.JobApplications
                .AnyAsync(a => a.JobPostId == job2.Id && a.JobSeekerId == seeker.Id);

            if (!application2Exists)
            {
                var application2 = new JobApplication(job2.Id, seeker.Id);
                context.JobApplications.Add(application2);
            }

            await context.SaveChangesAsync();

            Console.WriteLine("JobApplications Done");
            Console.WriteLine("Seeder Finished");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }
}