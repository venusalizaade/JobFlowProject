using JobFlowProject.Business.Dto.Feature;

namespace JobFlowProject.Business.Interfaces.User;

    public interface ICompanyFeatureService
    {
        Task AssignFeatureToCompanyAsync(AssignFeatureToCompanyDto dto);

        Task ExtendCompanyFeatureAsync(
            Guid companyFeatureId,
            DateTime newEndDate,
            Guid requesterId
        );

        Task RemoveFeatureFromCompanyAsync(
            Guid companyFeatureId,
            Guid requesterId
        );

        Task<List<FeatureListDto>> GetCompanyFeaturesAsync(Guid companyId);

        Task<List<CompanyFeatureDashboardDto>> GetCompanyFeatureDashboardAsync(Guid companyId);
    }
