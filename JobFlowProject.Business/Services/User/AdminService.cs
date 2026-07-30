using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Dto.Admin;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Dto.User;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Business.Services.MailKit;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Domain.Interfaces.Repository.User;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace JobFlowProject.Business.Services.User;


public class AdminService : IAdminService
{
    
    
        private readonly UserManager<AppUser> _userManager;
        private readonly JobFlowDbContext _context;
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IEmailService _emailService;
        
        private readonly IOptions<EmailSetting> _emailOptions;

        public AdminService(
            UserManager<AppUser> userManager,
            JobFlowDbContext context,
            IJobPostRepository jobPostRepository,
            IEmailService emailService,
            IOptions<EmailSetting> emailOptions)
        {
            _userManager = userManager;
            _context = context;
            _jobPostRepository = jobPostRepository;
            _emailService = emailService;
            _emailOptions = emailOptions;
        }


        public async Task VerifyEmployerAsync(Guid employerId)
        {
            var employer = await _userManager.FindByIdAsync(employerId.ToString());

            if (employer is null)
                throw new UserNotFoundException();

            employer.IsApproved = true;

            var result = await _userManager.UpdateAsync(employer);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            _ = Task.Run(async () =>
            {
                try
                {
                    await _emailService.SendAsync(
                        employer.Email!,
                        "Employer account approved",
                        "<h3>Your employer account has been approved successfully.</h3>");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email sending failed: {ex.Message}");
                }
            });
        }

        public async Task RejectEmployerAsync(Guid employerId)
        {
            var employer = await _userManager.FindByIdAsync(employerId.ToString());

            if (employer is null)
                throw new UserNotFoundException();

            employer.IsApproved = false;

            var result = await _userManager.UpdateAsync(employer);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            _ = Task.Run(async () =>
            {
                try
                {
                    await _emailService.SendAsync(
                        employer.Email!,
                        "Employer account rejected",
                        "<h3>Your employer account has been rejected.</h3>");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email sending failed: {ex.Message}");
                }
            });
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            var totalUsers = await _userManager.Users.CountAsync();

            var totalEmployers = (await _userManager
                .GetUsersInRoleAsync(RoleConstants.EmployerRoleName)).Count;

            var totalJobSeekers = (await _userManager
                .GetUsersInRoleAsync(RoleConstants.JobSeekerRoleName)).Count;

            var totalCompanies = await _context.Companies.CountAsync();

            var totalJobPosts = await _context.JobPosts.CountAsync();

            var employers = await _userManager
                .GetUsersInRoleAsync(RoleConstants.EmployerRoleName);

            var pendingEmployers = employers.Count(x => !x.IsApproved);

            var pendingApplications = await _context.JobApplications
                .CountAsync(x => x.Status == JobApplicationStatusEnum.Pending);

            return new DashboardDto(
                totalUsers,
                totalEmployers,
                totalJobSeekers,
                totalCompanies,
                totalJobPosts,
                pendingEmployers,
                pendingApplications);
        }

        public async Task<List<EmployerListDto>> GetEmployersAsync()
        {
            return await _userManager.Users
                .Where(x => x.CompanyId != null && !x.IsDeleted)
                .Select(x => new EmployerListDto(
                    x.Id,
                    $"{x.FirstName} {x.LastName}",
                    x.Email!,
                    x.PhoneNumber!,
                    x.Company != null ? x.Company.Name : null,
                    x.IsApproved))
                .ToListAsync();
        }

        public async Task<EmployerDetailsDto> GetEmployerDetailsAsync(Guid employerId)
        {
            var employer = await _userManager.Users
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == employerId && !x.IsDeleted);

            if (employer is null)
                throw new UserNotFoundException();

            return new EmployerDetailsDto(
                employer.Id,
                $"{employer.FirstName} {employer.LastName}",
                employer.Email!,
                employer.PhoneNumber!,
                employer.IsApproved,
                employer.Company?.Name,
                employer.Company?.NationalId,
                employer.Company?.Address,
                employer.Company?.About
            );
        }

        public async Task<List<JobSeekerListDto>> GetJobSeekersAsync()
        {
            return await _userManager.Users
                .Where(x => x.CompanyId == null && !x.IsDeleted)
                .Select(x => new JobSeekerListDto(
                    x.Id,
                    $"{x.FirstName} {x.LastName}",
                    x.Email!,
                    x.PhoneNumber!,
                    x.Gender,
                    x.About))
                .ToListAsync();
        }

        public async Task<JobSeekerDetailsDto> GetJobSeekerDetailsAsync(Guid jobSeekerId)
        {
            var user = await _userManager.Users
                .Include(x => x.Attachments)
                .Include(x => x.JobApplications)
                .ThenInclude(x => x.JobPost)
                .ThenInclude(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == jobSeekerId && !x.IsDeleted);

            if (user is null)
                throw new UserNotFoundException();

            return new JobSeekerDetailsDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email!,
                user.PhoneNumber!,
                user.Gender,
                user.NationalId,
                user.About,
                user.Attachments.Select(a => new JobSeekerAttachmentDto(
                    a.Id,
                    a.FileName,
                    a.FilePath
                )).ToList()!,
                user.JobApplications.Select(a => new JobSeekerApplicationDto(
                    a.Id,
                    a.JobPost.Title,
                    a.JobPost.Company.Name,
                    a.Status,
                    a.CreatedAt
                )).ToList()!
            );
        }

        public async Task<List<JobPostListDto>> GetJobPostsAsync()
        {
            var jobs = await _jobPostRepository.GetAllAsync();

            return jobs.Select(x => new JobPostListDto(
                x.Id,
                x.Title,
                x.Company.Name,
                x.Category.Name,
                x.City.Name,
                x.Salary,
                x.IsActive
            )).ToList();
        }

        public async Task DeleteJobPostAsync(Guid id, Guid requesterId)
        {
            var job = await _jobPostRepository.GetByIdAsync(id);

            if (job is null)
                throw new ItemNotFoundException();

            await _jobPostRepository.SoftDeleteAsync(id, requesterId);
        }

        public async Task DeleteJobSeekerAsync(Guid id, Guid requesterId)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                throw new UserNotFoundException();

            user.SetAsDeleted(requesterId);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);
        }
        public async Task ToggleJobPostStatusAsync(Guid jobPostId, Guid requesterId)
        {
            await _jobPostRepository.ToggleActiveAsync(jobPostId, requesterId);
        }
        
        public async Task SetJobPostFeaturedAsync(Guid jobPostId, int durationDays, Guid requesterId)
        {
            if (durationDays <= 0)
                throw new Exception("DurationDays must be greater than zero.");

            await _jobPostRepository.SetFeaturedAsync(
                jobPostId,
                durationDays,
                requesterId);
        }

        public async Task RemoveJobPostFeaturedAsync(Guid jobPostId, Guid requesterId)
        {
            await _jobPostRepository.RemoveFeaturedAsync(
                jobPostId,
                requesterId);
        }
        
        public async Task DisableJobSeekerAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                throw new UserNotFoundException();

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);
        }

        public async Task EnableJobSeekerAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
                throw new UserNotFoundException();

            user.LockoutEnd = null;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);
        }
        
       
        
        public Task<EmailSettingDto> GetEmailSettingAsync()
        {
            var s = _emailOptions.Value;

            return Task.FromResult(new EmailSettingDto(
                s.Host,
                s.Port,
                s.EnableSsl,
                s.Username,
                s.SenderName,
                s.SenderEmail));
        }
               
    

        public Task UpdateEmailSettingAsync(UpdateEmailSettingDto dto)
        {
            throw new NotImplementedException(
                "Updating appsettings.json at runtime is not supported.");
        }

    }
