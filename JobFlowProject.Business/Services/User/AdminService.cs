using JobFlowProject.Business.Constants;
using JobFlowProject.Business.Dto.Admin;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Dto.User;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Interfaces.Log;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Business.Services.EmailSender;
using JobFlowProject.Business.Services.MailKit;
using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Domain.Interfaces.Repository.User;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JobFlowProject.Business.Services.User;

public class AdminService : IAdminService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JobFlowDbContext _context;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly IEmailService _emailService;
    private readonly INotificationService _notificationService;

    private readonly IOptions<EmailSetting> _emailOptions;
    private readonly IServiceScopeFactory _scopeFactory;

    public AdminService(
        UserManager<AppUser> userManager,
        JobFlowDbContext context,
        IJobPostRepository jobPostRepository,
        IEmailService emailService,
        INotificationService notificationService,
        IOptions<EmailSetting> emailOptions,
        IServiceScopeFactory scopeFactory)
    {
        _userManager = userManager;
        _context = context;
        _jobPostRepository = jobPostRepository;
        _emailService = emailService;
        _notificationService = notificationService;
        _emailOptions = emailOptions;
        _scopeFactory = scopeFactory;
    }


    public async Task VerifyEmployerAsync(Guid employerId)
    {
        var employer = await _userManager.Users
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == employerId);

        if (employer is null)
            throw new UserNotFoundException();

        employer.IsApproved = true;

        var result = await _userManager.UpdateAsync(employer);

        if (!result.Succeeded)
            throw new Exception(result.Errors.First().Description);

        await _notificationService.NotifyAsync(
            employer.Id,
            "تأیید حساب کارفرما",
            "حساب کارفرمایی شما تأیید شد. اکنون می‌توانید آگهی ثبت کنید.",
            NotificationTypeEnum.EmployerVerified);

        _ = FireAndForgetEmailAsync(
                employer.Email!,
                "تأیید حساب کارفرما",
                EmailTemplates.EmployerApproved(employer.Company?.Name ?? "شرکت شما"));
    }

    public async Task RejectEmployerAsync(Guid employerId)
    {
        var employer = await _userManager.Users
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == employerId);

        if (employer is null)
            throw new UserNotFoundException();

        employer.IsApproved = false;

        var result = await _userManager.UpdateAsync(employer);

        if (!result.Succeeded)
            throw new Exception(result.Errors.First().Description);

        await _notificationService.NotifyAsync(
            employer.Id,
            "عدم تأیید حساب کارفرما",
            "حساب کارفرمایی شما تأیید نشد. در صورت نیاز با پشتیبانی تماس بگیرید.",
            NotificationTypeEnum.EmployerRejected);

        _ = FireAndForgetEmailAsync(
            employer.Email!,
            "عدم تأیید حساب کارفرما",
            EmailTemplates.EmployerRejected(employer.Company?.Name ?? "شرکت شما"));
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var totalUsers = await _userManager.Users.CountAsync();

        var totalEmployers = (await _userManager
            .GetUsersInRoleAsync(RoleConstants.EmployerRoleName)).Count;

        var totalJobSeekers = (await _userManager
            .GetUsersInRoleAsync(RoleConstants.JobSeekerRoleName)).Count;

        var totalCompanies = await _context.Companies.CountAsync(x => !x.IsDeleted);
        var totalJobPosts = await _context.JobPosts.CountAsync(x => !x.IsDeleted);

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
                x.Company != null && !x.Company.IsDeleted ? x.Company.Name : null,
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
        var seekers = await _userManager.GetUsersInRoleAsync(RoleConstants.JobSeekerRoleName);

        return seekers
            .Where(x => !x.IsDeleted)
            .Select(x => new JobSeekerListDto(
                x.Id,
                $"{x.FirstName} {x.LastName}",
                x.Email!,
                x.PhoneNumber!,
                x.Gender,
                x.About))
            .ToList();
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
        var jobs = await _context.JobPosts
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Company)
            .Include(x => x.Category)
            .Include(x => x.City)
            .Include(x => x.JobFeatures)
                .ThenInclude(jf => jf.Feature)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return jobs.Select(x => new JobPostListDto(
            x.Id,
            x.Title,
            x.Company.Name,
            x.Category.Name,
            x.City.Name,
            x.Salary,
            x.IsActive,
            x.JobFeatures
                .Where(jf => !jf.IsDeleted && jf.Status == Domain.Enums.JobFeatureStatusEnum.Active)
                .Select(jf => jf.Feature.Name)
                .ToList()
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
        var job = await _context.JobPosts
            .Include(x => x.Company)
            .ThenInclude(c => c.AppUser)
            .FirstOrDefaultAsync(x => x.Id == jobPostId && !x.IsDeleted);

        if (job is null)
            throw new ItemNotFoundException();

        var before = job.IsActive;

        await _jobPostRepository.ToggleActiveAsync(jobPostId, requesterId);

        if (before == job.IsActive)
            return;

        var owner = job.Company?.AppUser;

        if (owner is null || string.IsNullOrWhiteSpace(owner.Email))
            return;

        if (job.IsActive)
        {
            await _notificationService.NotifyAsync(
                owner.Id,
                "تأیید آگهی",
                $"آگهی «{job.Title}» توسط مدیر سامانه تأیید شد و هم‌اکنون فعال است.",
                NotificationTypeEnum.JobPostVerified,
                companyId: job.CompanyId,
                referenceId: jobPostId);

            _ = FireAndForgetEmailAsync(
                owner.Email,
                "تأیید آگهی شغلی",
                EmailTemplates.JobVerified(job.Title));
        }
        else
        {
            await _notificationService.NotifyAsync(
                owner.Id,
                "غیرفعال شدن آگهی",
                $"آگهی «{job.Title}» توسط مدیر سامانه غیرفعال شد.",
                NotificationTypeEnum.JobPostExpired,
                companyId: job.CompanyId,
                referenceId: jobPostId);

            _ = FireAndForgetEmailAsync(
                owner.Email,
                "غیرفعال شدن آگهی",
                EmailTemplates.JobRejected(job.Title));
        }
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
        var setting = _emailOptions.Value;

        return Task.FromResult(new EmailSettingDto(
            setting.Host,
            setting.Port,
            setting.EnableSsl,
            setting.Username,
            setting.SenderName,
            setting.SenderEmail));
    }

    public Task UpdateEmailSettingAsync(UpdateEmailSettingDto dto)
    {
        var email = _emailOptions.Value;

        return Task.FromResult(new UpdateEmailSettingDto(
            email.Host,
            email.Port,
            email.EnableSsl,
            email.Username,
            email.Password,
            email.SenderName,
            email.SenderEmail
        ));


    }

    public async Task<List<PaymentListDto>> GetPaymentsAsync()
    {
        return await _context.Payments
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .Include(p => p.JobPost)
            .ThenInclude(j => j.Company)
            .Include(p => p.Feature)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PaymentListDto(
                p.Id,
                p.JobPost.Company.Name,
                p.JobPost.Title,
                p.Feature.Name,
                p.Amount,
                p.Status,
                p.CreatedAt))
            .ToListAsync();
    }

    public async Task ConfirmPaymentAsync(Guid paymentId, Guid requesterId)
    {        var payment = await _context.Payments
            .Include(p => p.JobPost)
            .ThenInclude(j => j.Company)
            .Include(p => p.Feature)
            .FirstOrDefaultAsync(p => p.Id == paymentId && !p.IsDeleted);

        if (payment is null)
            throw new ItemNotFoundException("Payment not found.");

        if (payment.Status != PaymentStatusEnum.Success)
            throw new Exception("فقط پرداخت‌های در انتظار تأیید قابل تأیید هستند.");

        var companyId = payment.JobPost.CompanyId;
        var featureId = payment.FeatureId;

        var alreadyAssigned = await _context.CompanyFeatures
            .AnyAsync(cf => cf.CompanyId == companyId && cf.FeatureId == featureId && !cf.IsDeleted);

        if (!alreadyAssigned)
        {
            var companyFeature = new CompanyFeature(
                companyId,
                featureId,
                DateTime.UtcNow,
                DateTime.UtcNow.AddDays(payment.Feature.DurationDays));

            _context.CompanyFeatures.Add(companyFeature);
        }

        payment.JobPost.SetFeatured(payment.Feature.DurationDays, requesterId);
        payment.Confirm();

        _context.NotificationLogs.Add(new NotificationLog(
            "تأیید فیچر",
            $"فیچر «{payment.Feature.Name}» برای آگهی «{payment.JobPost.Title}» تأیید و فعال شد.",
            NotificationTypeEnum.PaymentConfirmed,
            payment.JobPost.Company.AppUserId,
            companyId,
            payment.JobPostId));

        await _context.SaveChangesAsync();

        var owner = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == payment.JobPost.Company.AppUserId);

        if (owner is not null && !string.IsNullOrWhiteSpace(owner.Email))
        {
            _ = FireAndForgetEmailAsync(
                owner.Email,
                "تأیید پرداخت فیچر",
                EmailTemplates.PaymentConfirmed(payment.Feature.Name, payment.JobPost.Title));
        }
    }

    public async Task RejectPaymentAsync(Guid paymentId, Guid requesterId)
    {
        var payment = await _context.Payments
            .Include(p => p.JobPost)
            .ThenInclude(j => j.Company)
            .Include(p => p.Feature)
            .FirstOrDefaultAsync(p => p.Id == paymentId && !p.IsDeleted);

        if (payment is null)
            throw new ItemNotFoundException("Payment not found.");

        if (payment.Status != PaymentStatusEnum.Success)
            throw new Exception("فقط پرداخت‌های در انتظار تأیید قابل رد شدن هستند.");

        payment.MarkAsFailed();
        payment.SetModificationInfo(requesterId);

        _context.NotificationLogs.Add(new NotificationLog(
            "رد پرداخت فیچر",
            $"پرداخت فیچر «{payment.Feature.Name}» برای آگهی «{payment.JobPost.Title}» تأیید نشد.",
            NotificationTypeEnum.FeaturePurchaseRequest,
            payment.JobPost.Company.AppUserId,
            payment.JobPost.CompanyId,
            payment.JobPostId));

        await _context.SaveChangesAsync();

        var owner = await _userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == payment.JobPost.Company.AppUserId);

        if (owner is not null && !string.IsNullOrWhiteSpace(owner.Email))
        {
            _ = FireAndForgetEmailAsync(
                owner.Email,
                "عدم تأیید پرداخت فیچر",
                EmailTemplates.PaymentRejected(payment.Feature.Name, payment.JobPost.Title));
        }
    }

    private async Task FireAndForgetEmailAsync(string to, string subject, string body)
    {        try
        {
            
            using var scope = _scopeFactory.CreateScope();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            
            await emailService.SendAsync(to, subject, body);
        }
        catch (Exception ex)
        {
           
            Console.WriteLine($"[Email Failed] To: {to}, Error: {ex.Message}");
        }
    }
}
