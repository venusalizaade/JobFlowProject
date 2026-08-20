using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Interfaces.Log;
using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;

namespace JobFlowProject.Business.Services.CompaneisService;

public class FeatureService : IFeatureService
{
    private readonly IFeatureRepository _featureRepository;
    private readonly INotificationService _notificationService;

    public FeatureService(
        IFeatureRepository featureRepository,
        INotificationService notificationService)
    {
        _featureRepository = featureRepository;
        _notificationService = notificationService;
    }

    private static FeatureListDto ToListDto(Feature x)
    {
        return new FeatureListDto(
            x.Id,
            x.Name,
            x.Description,
            x.Price,
            x.DurationDays,
            x.IsActive,
            x.FeatureType);
    }

    public async Task<List<FeatureListDto>> GetFeaturesAsync()
    {
        var features = await _featureRepository.GetAllAsync();

        return features.Select(ToListDto).ToList();
    }

    public async Task<List<FeatureListDto>> GetActiveFeaturesAsync()
    {
        var features = await _featureRepository.GetActiveAsync();

        return features.Select(ToListDto).ToList();
    }

    public async Task CreateFeatureAsync(CreateFeatureDto dto)
    {
        if (await _featureRepository.GetByNameAsync(dto.Name) is not null)
            throw new Exception("Feature already exists.");

        var feature = new Feature(
            dto.Name,
            dto.Price,
            dto.DurationDays,
            dto.FeatureType,
            dto.Description);

        await _featureRepository.AddAsync(feature);

        await _notificationService.NotifyAdminAsync(
            "فیچر جدید ایجاد شد",
            $"فیچر «{dto.Name}» با قیمت {dto.Price:N0} تومان ایجاد شد.",
            NotificationTypeEnum.System);
    }

    public async Task UpdateFeatureAsync(Guid id, UpdateFeatureDto dto)
    {
        var feature = await _featureRepository.GetByIdAsync(id);

        if (feature is null)
            throw new Exception("Feature not found.");

        feature.Name = dto.Name;
        feature.Description = dto.Description;
        feature.Price = dto.Price;
        feature.DurationDays = dto.DurationDays;
        feature.FeatureType = dto.FeatureType;
        feature.IsActive = dto.IsActive;

        feature.Validate();

        await _featureRepository.UpdateAsync(feature);

        await _notificationService.NotifyAdminAsync(
            "ویرایش فیچر",
            $"فیچر «{dto.Name}» ویرایش شد.",
            NotificationTypeEnum.System);
    }

    public async Task DeleteFeatureAsync(Guid id, Guid requesterId)
    {
        var feature = await _featureRepository.GetByIdAsync(id);

        if (feature is null)
            throw new Exception("Feature not found.");

        await _featureRepository.SoftDeleteAsync(id, requesterId);

        await _notificationService.NotifyAdminAsync(
            "حذف فیچر",
            $"فیچر «{feature.Name}» حذف شد.",
            NotificationTypeEnum.System);
    }

    public async Task ToggleFeatureActiveAsync(Guid id, Guid requesterId)
    {
        var feature = await _featureRepository.GetByIdAsync(id);

        if (feature is null)
            throw new Exception("Feature not found.");

        feature.ToggleActive();
        feature.SetModificationInfo(requesterId);

        await _featureRepository.UpdateAsync(feature);

        await _notificationService.NotifyAdminAsync(
            "تغییر وضعیت فیچر",
            $"وضعیت نمایش فیچر «{feature.Name}» تغییر کرد.",
            NotificationTypeEnum.System);
    }
}
