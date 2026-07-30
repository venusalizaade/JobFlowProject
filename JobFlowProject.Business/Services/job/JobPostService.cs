using JobFlowProject.Business.Dto.Commands;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Dto.User;
using JobFlowProject.Business.Exceptions.Authentication_Exceptions;
using JobFlowProject.Business.Exceptions.AuthenticationExceptions;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.JobPost;
using JobFlowProject.Domain.Entities.Componies;
using JobFlowProject.Domain.Entities.Job;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Dto.Authentication;
using JobPostResponseDto = JobFlowProject.Business.Dto.JobPost.JobPostResponseDto;
using JobPostSearchRequestDto = JobFlowProject.Business.Dto.JobPost.JobPostSearchRequestDto;

namespace JobFlowProject.Business.Services.job;

public class JobPostService : IJobPostService
{
    private readonly IJobPostRepository _jobPostRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly UserManager<AppUser> _userManager;

    public JobPostService(
        IJobPostRepository jobPostRepository,
        ICompanyRepository companyRepository,
        UserManager<AppUser> userManager)
    {
        _jobPostRepository = jobPostRepository;
        _companyRepository = companyRepository;
        _userManager = userManager;
    }

    public async Task<JobPostResponseDto> CreateAsync(
        Guid requesterId,
        CreateJobPostCommand command)
    {
        var employer = await _userManager.FindByIdAsync(requesterId.ToString());

        if (employer is null)
            throw new UserNotFoundException();
        

        var company = await _companyRepository.GetByAppUserIdAsync(requesterId);

        if (company is null)
            throw new ItemNotFoundException("Company not found.");

        var jobPost = new JobPost(
            command.Title,
            command.AboutJob,
            command.ProvinceId,
            command.CityId,
            command.EmploymentType,
            command.Salary,
            company.Id,
            command.CategoryId,
            command.SkillId
            );

        

        jobPost.Validate();
        await _jobPostRepository.AddAsync(jobPost);

        return new JobPostResponseDto(
            jobPost.Id,
            jobPost.Title,
            jobPost.AboutJob,
            jobPost.Salary,
            jobPost.EmploymentType,
            jobPost.IsActive,
            jobPost.ExpiresAt);
    }

    public async Task<List<JobPostResponseDto>> GetCompanyJobPostsAsync(Guid requesterId)
    {
        var company = await _companyRepository.GetByAppUserIdAsync(requesterId);

        if (company is null)
            throw new ItemNotFoundException("Company not found.");

        var jobs = await _jobPostRepository.GetCompanyJobPostsAsync(company.Id);

        return jobs.Select(job => new JobPostResponseDto(
            job.Id,
            job.Title,
            job.AboutJob,
            job.Salary,
            job.EmploymentType,
            job.IsActive,
            job.ExpiresAt
        )).ToList();
    }

    public async Task<JobPostDetailDto> GetDetailsAsync(Guid id)
    {
        var job = await _jobPostRepository.GetJobPostDetailsAsync(id);

        if (job is null)
            throw new ItemNotFoundException("Job post not found.");

        return new JobPostDetailDto(
            job.Id,
            job.Title,
            job.AboutJob,
            job.Salary,
            job.EmploymentType,
            job.IsActive,
            job.ExpiresAt,
            job.Company.Name,
            job.Category.Name,
            job.City.Name,
            job.City.Province.Name
        );
    }

    public async Task UpdateAsync(
        Guid requesterId,
        Guid jobPostId,
        UpdateJobPostCommand command)
    {
        var company = await _companyRepository.GetByAppUserIdAsync(requesterId);

        if (company is null)
            throw new ItemNotFoundException("Company not found.");

        var jobPost = await _jobPostRepository.GetByIdAsync(jobPostId);

        if (jobPost is null)
            throw new ItemNotFoundException("Job post not found.");

        if (jobPost.CompanyId != company.Id)
            throw new PermissionDeniedException();

        jobPost.Title = command.Title;
        jobPost.AboutJob = command.AboutJob;
        jobPost.CityId = command.CityId;
        jobPost.ProvinceId = command.ProvinceId;
        jobPost.CategoryId = command.CategoryId;
        jobPost.EmploymentType = command.EmploymentType;
        jobPost.Salary = command.Salary;
        jobPost.SkillId = command.SkillId;

        jobPost.Validate();

        await _jobPostRepository.UpdateAsync(jobPost);
    }
    

    public async Task DeactivateAsync(
        Guid requesterId,
        Guid jobPostId)
    {
        var company = await _companyRepository.GetByAppUserIdAsync(requesterId);

        if (company is null)
            throw new ItemNotFoundException("Company not found.");

        var jobPost = await _jobPostRepository.GetByIdAsync(jobPostId);

        if (jobPost is null)
            throw new ItemNotFoundException("Job post not found.");

        if (jobPost.CompanyId != company.Id)
            throw new PermissionDeniedException();

        jobPost.Deactivate();

        await _jobPostRepository.UpdateAsync(jobPost);
    }
    
    public async Task<List<JobPostResponseDto>> GetActiveAsync()
    {
        var jobs = await _jobPostRepository.GetActiveAsync();

        return jobs.Select(job => new JobPostResponseDto(
            job.Id,
            job.Title,
            job.AboutJob,
            job.Salary,
            job.EmploymentType,
            job.IsActive,
            job.ExpiresAt
        )).ToList();
    }
    
    public async Task<List<JobPostResponseDto>> SearchAsync(JobPostSearchRequestDto dto)
    {
        var jobs = await _jobPostRepository.SearchAsync(
            dto.Title,
            dto.CategoryId,
            dto.SkillId,
            dto.EmploymentType,
            dto.MinSalary,
            dto.MaxSalary,
            dto.CityId,
            dto.ProvinceId);

        return jobs.Select(job => new JobPostResponseDto(
            job.Id,
            job.Title,
            job.AboutJob,
            job.Salary,
            job.EmploymentType,
            job.IsActive,
            job.ExpiresAt
        )).ToList();
    }
    
   
    public async Task<List<JobPostResponseDto>> FilterAsync(JobPostFilterRequestDto dto)
    {
        var jobs = await _jobPostRepository.FilterAsync(
            dto.CategoryId,
            dto.SkillId,
            dto.MinSalary,
            dto.MaxSalary);

        return jobs.Select(job => new JobPostResponseDto(
            job.Id,
            job.Title,
            job.AboutJob,
            job.Salary,
            job.EmploymentType,
            job.IsActive,
            job.ExpiresAt
        )).ToList();
    }
}