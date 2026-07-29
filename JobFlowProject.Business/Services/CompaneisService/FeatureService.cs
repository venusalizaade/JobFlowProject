using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Domain.Interfaces.Repository;

namespace JobFlowProject.Business.Services.CompaneisService;

public class FeatureService
{
    private readonly IFeatureRepository _featureRepository;

    public FeatureService(IFeatureRepository  featureRepository)
    {
        _featureRepository = featureRepository;
    }

    public async Task<List<FeatureListDto>> GetFeaturesAsync()
    {
        var features = await _featureRepository.GetAllAsync();

        return features.Select(x => new FeatureListDto(
            x.Id,
            x.Name,
            x.Price,
            x.DurationDays
        )).ToList();
    }
    public async Task CreateFeatureAsync(CreateFeatureDto dto)
    {
        if (await _featureRepository.GetByNameAsync(dto.Name) is not null)
            throw new Exception("Feature already exists.");

        var feature = new Feature(
            dto.Name,
            dto.Price,
            dto.DurationDays,
            dto.FeatureType);

        await _featureRepository.AddAsync(feature);
    }
    public async Task UpdateFeatureAsync(Guid id, UpdateFeatureDto dto)
    {
        var feature = await _featureRepository.GetByIdAsync(id);

        if (feature is null)
            throw new Exception("Feature not found.");

        feature.Name = dto.Name;
        feature.Price = dto.Price;
        feature.DurationDays = dto.DurationDays;
        feature.FeatureType = dto.FeatureType;

        feature.Validate();

        await _featureRepository.UpdateAsync(feature);
    }
 
    public async Task DeleteFeatureAsync(Guid id, Guid requesterId)
    {
        await _featureRepository.SoftDeleteAsync(id, requesterId);
    }
}