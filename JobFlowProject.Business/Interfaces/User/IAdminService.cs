using JobFlowProject.Business.Dto.Admin;
using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Dto.User;

namespace JobFlowProject.Business.Interfaces.User;

public interface IAdminService
{
    Task VerifyEmployerAsync(Guid employerId);

    Task RejectEmployerAsync(Guid employerId);

    Task<DashboardDto> GetDashboardAsync();

    Task<List<EmployerListDto>> GetEmployersAsync();

    Task<EmployerDetailsDto> GetEmployerDetailsAsync(Guid employerId);

    Task<List<JobSeekerListDto>> GetJobSeekersAsync();

    Task<JobSeekerDetailsDto> GetJobSeekerDetailsAsync(Guid jobSeekerId);

    Task<List<JobPostListDto>> GetJobPostsAsync();

    Task DeleteJobPostAsync(Guid id, Guid requesterId);

    Task DeleteJobSeekerAsync(Guid id, Guid requesterId);
    Task ToggleJobPostStatusAsync(Guid jobPostId, Guid requesterId);
    
    Task SetJobPostFeaturedAsync(Guid jobPostId, int durationDays, Guid requesterId);

    Task RemoveJobPostFeaturedAsync(Guid jobPostId, Guid requesterId);
    
    Task DisableJobSeekerAsync(Guid id);

    Task EnableJobSeekerAsync(Guid id);
    
    Task<EmailSettingDto> GetEmailSettingAsync();

    Task UpdateEmailSettingAsync(UpdateEmailSettingDto dto);

    Task<List<PaymentListDto>> GetPaymentsAsync();

    Task ConfirmPaymentAsync(Guid paymentId, Guid requesterId);
}
