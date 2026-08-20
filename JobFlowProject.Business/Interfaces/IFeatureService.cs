using JobFlowProject.Business.Dto.Feature;

namespace JobFlowProject.Business.Interfaces;

public interface IFeatureService
{
    Task<List<FeatureListDto>> GetFeaturesAsync();

    Task<List<FeatureListDto>> GetActiveFeaturesAsync();

    Task CreateFeatureAsync(CreateFeatureDto dto);

    Task UpdateFeatureAsync(Guid id, UpdateFeatureDto dto);

    Task DeleteFeatureAsync(Guid id, Guid requesterId);

    Task ToggleFeatureActiveAsync(Guid id, Guid requesterId);
}
