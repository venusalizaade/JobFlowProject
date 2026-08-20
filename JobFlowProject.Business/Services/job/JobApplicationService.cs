using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.AuthenticationExceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Business.Interfaces.JobPost;
using JobFlowProject.Business.Services.EmailSender;
using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Entites.Logs;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;

namespace JobFlowProject.Business.Services.Job;

public class JobApplicationService : IJobApplicationService
{
private readonly IJobApplicationRepository _jobApplicationRepository;
private readonly IJobPostRepository _jobPostRepository;
private readonly ICompanyRepository _companyRepository;
private readonly UserManager<AppUser> _userManager;
private readonly IEmailService _emailService;
private readonly INotificationRepository _notificationRepository;


public JobApplicationService(
    IJobApplicationRepository jobApplicationRepository,
    IJobPostRepository jobPostRepository,
    ICompanyRepository companyRepository,
    UserManager<AppUser> userManager,
    IEmailService emailService,
    INotificationRepository notificationRepository)
{
    _jobApplicationRepository = jobApplicationRepository;
    _jobPostRepository = jobPostRepository;
    _companyRepository = companyRepository;
    _userManager = userManager;
    _emailService = emailService;
    _notificationRepository = notificationRepository;
}

public async Task ApplyAsync(Guid requesterId, ApplyJobCommand command)
{
    var applicant = await _userManager.FindByIdAsync(requesterId.ToString());

    if (applicant is null)
        throw new UserNotFoundException();

    var jobPost = await _jobPostRepository.GetByIdAsync(command.JobPostId);

    if (jobPost is null)
        throw new ItemNotFoundException("Job post not found.");

    if (!jobPost.IsActive || jobPost.ExpiresAt <= DateTime.UtcNow)
        throw new PermissionDeniedException();

    var exists = await _jobApplicationRepository.HasAppliedAsync(command.JobPostId, requesterId);

    if (exists)
        throw new Exception("You have already applied for this job.");

    var application = new JobApplication(command.JobPostId, requesterId);

    await _jobApplicationRepository.AddAsync(application);

    var company = await _companyRepository.GetByCompanyIdAsync(jobPost.CompanyId);
    var employer = company is null
        ? null
        : await _userManager.FindByIdAsync(company.AppUserId.ToString());

    try
    {
        if (employer is not null && !string.IsNullOrWhiteSpace(employer.Email))
        {
            var subject = "رزومه جدید برای آگهی شما";
            var body = EmailTemplates.NewResume(
                jobPost.Title,
                $"{applicant.FirstName} {applicant.LastName}",
                company?.Name ?? "شرکت");

            await _emailService.SendAsync(employer.Email, subject, body);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Employer email failed: {ex.Message}");
    }

    try
    {
        if (!string.IsNullOrWhiteSpace(applicant.Email))
        {
            var subject = "ثبت درخواست با موفقیت";
            var body = EmailTemplates.ApplicationSubmitted(
                jobPost.Title,
                company?.Name ?? "شرکت");

            await _emailService.SendAsync(applicant.Email, subject, body);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Jobseeker email failed: {ex.Message}");
    }

    if (employer is not null)
    {
        try
        {
            var notification = new NotificationLog(
                "رزومه جدید دریافت شد",
                $"کاربر {applicant.FirstName} {applicant.LastName} برای آگهی «{jobPost.Title}» رزومه ارسال کرد.",
                NotificationTypeEnum.ResumeReceived,
                employer.Id,
                jobPost.CompanyId,
                jobPost.Id);

            await _notificationRepository.AddAsync(notification);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Notification failed: {ex.Message}");
        }
    }
}

public async Task<List<JobApplicationDto>> GetJobApplicationsAsync(Guid requesterId, Guid jobPostId)
{
    var company = await _companyRepository.GetByAppUserIdAsync(requesterId);

    if (company is null)
        throw new ItemNotFoundException("Company not found.");

    var jobPost = await _jobPostRepository.GetByIdAsync(jobPostId);

    if (jobPost is null)
        throw new ItemNotFoundException("Job post not found.");

    if (jobPost.CompanyId != company.Id)
        throw new PermissionDeniedException();

    var applications = await _jobApplicationRepository.GetByJobPostAsync(jobPostId);

    return applications
        .Select(x => new JobApplicationDto(
            x.Id,
            x.JobPostId,
            x.JobSeekerId,
            $"{x.JobSeeker.FirstName} {x.JobSeeker.LastName}",
            x.Status))
        .ToList();
}

public async Task ChangeStatusAsync(Guid requesterId, ChangeApplicationStatusCommand command)
{
    var company = await _companyRepository.GetByAppUserIdAsync(requesterId);

    if (company is null)
        throw new ItemNotFoundException("Company not found.");

    var application = await _jobApplicationRepository.GetDetailsAsync(command.JobApplicationId);

    if (application is null)
        throw new ItemNotFoundException("Application not found.");

    if (application.JobPost.CompanyId != company.Id)
        throw new PermissionDeniedException();

    var finalStatuses = new[]
    {
        JobApplicationStatusEnum.Interview,
        JobApplicationStatusEnum.Accepted,
        JobApplicationStatusEnum.Rejected
    };

    if (finalStatuses.Contains(application.Status) &&
        command.Status is JobApplicationStatusEnum.Review or JobApplicationStatusEnum.Pending)  
        throw new StatusChangeNotAllowedException();

    application.ChangeStatus(command.Status);
    application.SetModificationInfo(requesterId);

    await _jobApplicationRepository.UpdateAsync(application);

    try
    {
        string subject = command.Status switch
        {
            JobApplicationStatusEnum.Accepted => "درخواست شما پذیرفته شد",
            JobApplicationStatusEnum.Rejected => "درخواست شما رد شد",
            JobApplicationStatusEnum.Interview => "دعوت به مصاحبه",
            JobApplicationStatusEnum.Review => "درخواست شما در حال بررسی است",
            JobApplicationStatusEnum.Cancelled => "درخواست شما لغو شد",
            _ => "به‌روزرسانی وضعیت درخواست"
        };

        string statusFa = command.Status switch
        {
            JobApplicationStatusEnum.Accepted => "پذیرفته شد",
            JobApplicationStatusEnum.Rejected => "رد شد",
            JobApplicationStatusEnum.Interview => "مصاحبه",
            JobApplicationStatusEnum.Review => "در حال بررسی",
            JobApplicationStatusEnum.Cancelled => "لغو شد",
            _ => "به‌روزرسانی شد"
        };

        string body = EmailTemplates.ApplicationStatus(
            application.JobPost.Title,
            application.JobPost.Company.Name,
            statusFa);

        await _emailService.SendAsync(
            application.JobSeeker.Email!,
            subject,
            body);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Email sending failed: {ex.Message}");
    }

    try
    {
        var statusNotification = command.Status switch
        {
            JobApplicationStatusEnum.Accepted => new NotificationLog(
                "درخواست شما پذیرفته شد",
                $"درخواست شما برای «{application.JobPost.Title}» توسط {application.JobPost.Company.Name} پذیرفته شد.",
                NotificationTypeEnum.ApplicationConfirmed,
                application.JobSeekerId,
                application.JobPost.CompanyId,
                application.Id),
            JobApplicationStatusEnum.Rejected => new NotificationLog(
                "درخواست شما رد شد",
                $"درخواست شما برای «{application.JobPost.Title}» رد شد.",
                NotificationTypeEnum.ApplicationReviewed,
                application.JobSeekerId,
                application.JobPost.CompanyId,
                application.Id),
            JobApplicationStatusEnum.Interview => new NotificationLog(
                "دعوت به مصاحبه",
                $"برای آگهی «{application.JobPost.Title}» به مصاحبه دعوت شدید.",
                NotificationTypeEnum.ApplicationReviewed,
                application.JobSeekerId,
                application.JobPost.CompanyId,
                application.Id),
            _ => null
        };

        if (statusNotification is not null)
            await _notificationRepository.AddAsync(statusNotification);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Notification failed: {ex.Message}");
    }
}



public async Task<List<JobApplicationResponseDto>> GetMyApplicationsAsync(Guid requesterId)
{
    var applications = await _jobApplicationRepository.GetByJobSeekerIdAsync(requesterId);

    return applications
        .Select(x => new JobApplicationResponseDto(
            x.Id,
            x.JobPost.Title,
            x.JobPost.Company != null && !x.JobPost.Company.IsDeleted ? x.JobPost.Company.Name : null,
            x.Status,
            x.CreatedAt))
        .ToList();
}

public async Task<JobApplicationDetailDto> GetDetailsAsync(Guid requesterId, Guid applicationId)
{
    var application = await _jobApplicationRepository.GetDetailsAsync(applicationId);

    if (application is null)
        throw new ItemNotFoundException("Application not found.");

    if (application.JobSeekerId != requesterId)
        throw new PermissionDeniedException();

    return new JobApplicationDetailDto(
        application.Id,
        application.JobPost.Title,
        application.JobPost.Company.Name,
        application.Status,
        application.CreatedAt);
}

public async Task CancelAsync(Guid requesterId, Guid applicationId)
{
    var application = await _jobApplicationRepository.GetPendingApplicationAsync(applicationId, requesterId);

    if (application is null)
        throw new ItemNotFoundException("Pending application not found.");

    application.ChangeStatus(JobApplicationStatusEnum.Cancelled);

    await _jobApplicationRepository.UpdateAsync(application);
}


}
