using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.AuthenticationExceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Entities.User;
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


public JobApplicationService(
    IJobApplicationRepository jobApplicationRepository,
    IJobPostRepository jobPostRepository,
    ICompanyRepository companyRepository,
    UserManager<AppUser> userManager,
    IEmailService emailService)
{
    _jobApplicationRepository = jobApplicationRepository;
    _jobPostRepository = jobPostRepository;
    _companyRepository = companyRepository;
    _userManager = userManager;
    _emailService = emailService;
}

public async Task ApplyAsync(Guid requesterId, ApplyJobCommand command)
{
    var applicant = await _userManager.FindByIdAsync(requesterId.ToString());

    if (applicant is null)
        throw new UserNotFoundException();

    var jobPost = await _jobPostRepository.GetByIdAsync(command.JobPostId);

    if (jobPost is null)
        throw new ItemNotFoundException("Job post not found.");

    if (!jobPost.IsActive)
        throw new PermissionDeniedException();

    var exists = await _jobApplicationRepository.HasAppliedAsync(command.JobPostId, requesterId);

    if (exists)
        throw new Exception("You have already applied for this job.");

    var application = new JobApplication(requesterId, command.JobPostId);

    await _jobApplicationRepository.AddAsync(application);
    try
    {
        var employer = await _userManager.FindByIdAsync(jobPost.Company.AppUserId.ToString());

        if (employer is not null && !string.IsNullOrWhiteSpace(employer.Email))
        {
            var subject = "New job application received";
            var body = "A new applicant has applied for your job post: " + jobPost.Title +
                       ". Applicant: " + applicant.FirstName + " " + applicant.LastName;

            await _emailService.SendAsync(employer.Email, subject, body);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Employer email failed: {ex.Message}");
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

    application.ChangeStatus(command.Status);
    application.SetModificationInfo(requesterId);

    await _jobApplicationRepository.UpdateAsync(application);

    try
    {
        string subject = command.Status switch
        {
            JobApplicationStatusEnum.Accepted => "Your job application has been accepted",
            JobApplicationStatusEnum.Rejected => "Your job application has been rejected",
            JobApplicationStatusEnum.Interview => "Interview invitation",
            JobApplicationStatusEnum.Review => "Your application is under review",
            JobApplicationStatusEnum.Cancelled => "Your application has been cancelled",
            _ => "Application status updated"
        };

        string body = command.Status switch
        {
            JobApplicationStatusEnum.Accepted =>
                $"<h3>Congratulations!</h3><p>Your application for <b>{application.JobPost.Title}</b> has been accepted by <b>{application.JobPost.Company.Name}</b>.</p>",

            JobApplicationStatusEnum.Rejected =>
                $"<h3>Application update</h3><p>Your application for <b>{application.JobPost.Title}</b> has been rejected by <b>{application.JobPost.Company.Name}</b>.</p>",

            JobApplicationStatusEnum.Interview =>
                $"<h3>Interview invitation</h3><p>You have been invited to an interview for <b>{application.JobPost.Title}</b> at <b>{application.JobPost.Company.Name}</b>.</p>",

            JobApplicationStatusEnum.Review =>
                $"<h3>Application under review</h3><p>Your application for <b>{application.JobPost.Title}</b> is currently being reviewed by <b>{application.JobPost.Company.Name}</b>.</p>",

            JobApplicationStatusEnum.Cancelled =>
                $"<h3>Application cancelled</h3><p>Your application for <b>{application.JobPost.Title}</b> has been cancelled.</p>",

            _ =>
                $"<p>The status of your application for <b>{application.JobPost.Title}</b> has been updated.</p>"
        };

        await _emailService.SendAsync(
            application.JobSeeker.Email!,
            subject,
            body);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Email sending failed: {ex.Message}");
    }
}

public async Task<List<JobApplicationResponseDto>> GetMyApplicationsAsync(Guid requesterId)
{
    var applications = await _jobApplicationRepository.GetByJobSeekerIdAsync(requesterId);

    return applications
        .Select(x => new JobApplicationResponseDto(
            x.Id,
            x.JobPost.Title,
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
