using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;

namespace JobFlowProject.Business.Interfaces.User;



    public interface ICompanyFeatureService
    {
        Task AssignFeatureToCompanyAsync(AssignFeatureToCompanyDto dto);

        Task RemoveFeatureFromCompanyAsync(
            Guid companyFeatureId,
            Guid requesterId
        );

        Task<List<FeatureListDto>> GetCompanyFeaturesAsync(Guid companyId);
    }

