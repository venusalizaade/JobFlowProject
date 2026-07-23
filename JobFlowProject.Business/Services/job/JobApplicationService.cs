using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.AuthenticationExceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Business.Services.job;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly IJobPostRepository _jobPostRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly UserManager<AppUser> _userManager;


    public JobApplicationService(
        IJobApplicationRepository jobApplicationRepository,
        IJobPostRepository jobPostRepository,
        ICompanyRepository companyRepository,
        UserManager<AppUser> userManager)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _jobPostRepository = jobPostRepository;
        _companyRepository = companyRepository;
        _userManager = userManager;
    }

    public async Task ApplyAsync(Guid requesterId, ApplyJobCommand command)
    {
        var applicant = await _userManager
            .FindByIdAsync(requesterId.ToString());

        if (applicant is null)
            throw new UserNotFoundException();


        var jobPost = await _jobPostRepository
            .GetByIdAsync(command.JobPostId);


        if (jobPost is null)
            throw new ItemNotFoundException("Job post not found.");


        if (!jobPost.IsActive)
            throw new PermissionDeniedException();


        var exists = await _jobApplicationRepository
            .ExistsAsync(command.JobPostId, requesterId);


        if (exists)
            throw new Exception("You already applied for this job.");


        var application = new JobApplication(requesterId,command.JobPostId );

         
        await _jobApplicationRepository.AddAsync(application);
    }

    public async Task<List<JobApplicationDto>> GetJobApplicationsAsync(Guid requesterId, Guid jobPostId)
    {
        var company = await _companyRepository
            .GetByAppUserIdAsync(requesterId);


        if (company is null)
            throw new ItemNotFoundException("Company not found.");


        var jobPost = await _jobPostRepository
            .GetByIdAsync(jobPostId);


        if (jobPost is null)
            throw new ItemNotFoundException("Job post not found.");


        if (jobPost.CompanyId != company.Id)
            throw new PermissionDeniedException();


        var applications = await _jobApplicationRepository
            .GetByJobPostAsync(jobPostId);


        return applications
            .Select(x => new JobApplicationDto(
                x.Id,
                x.JobPostId,
                x.JobSeekerId,
                x.JobSeeker.FirstName + " " + x.JobSeeker.LastName,
                x.Status))
            .ToList();

    }

    public async Task ChangeStatusAsync(Guid requesterId, ChangeApplicationStatusCommand command)
    {
        var company = await _companyRepository.GetByAppUserIdAsync(requesterId);
        if (company is null) throw new ItemNotFoundException("Company not found.");

        var application = await _jobApplicationRepository.GetByIdAsync(command.JobApplicationId);
        if (application is null) throw new ItemNotFoundException("Application not found.");

        if (application.JobPost.CompanyId != company.Id)
            throw new PermissionDeniedException();

        application.ChangeStatus(command.Status);

        await _jobApplicationRepository.UpdateAsync(application);
    }
     
    public async Task<List<JobApplicationResponseDto>> GetMyApplicationsAsync(
        Guid requesterId)
    {
        var applications =
            await _jobApplicationRepository.GetByJobSeekerIdAsync(requesterId);

        return applications.Select(x => new JobApplicationResponseDto(
            x.Id,
            x.JobPost.Title,
            x.Status,
            x.CreatedAt
        )).ToList();
    }

    public async Task<JobApplicationDetailDto> GetDetailsAsync(
        Guid requesterId,
        Guid applicationId)
    {
        var application =
            await _jobApplicationRepository.GetDetailsAsync(applicationId);

        if (application is null)
            throw new ItemNotFoundException("Application not found.");

        if (application.JobSeekerId != requesterId)
            throw new PermissionDeniedException();

        return new JobApplicationDetailDto(
            application.Id,
            application.JobPost.Title,
            application.JobPost.Company.Name,
            application.Status,
            application.CreatedAt
        );
    }
    public async Task CancelAsync(Guid requesterId, Guid applicationId)
    {
        var application =
            await _jobApplicationRepository.GetPendingApplicationAsync(
                applicationId,
                requesterId);

        if (application is null)
            throw new ItemNotFoundException("Pending application not found.");

        application.ChangeStatus(JobApplicationStatusEnum.Cancelled);

        await _jobApplicationRepository.UpdateAsync(application);
    }

}