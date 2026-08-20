using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.Log;
using JobFlowProject.Business.Interfaces;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Business.Services.EmailSender;
using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Domain.Entities.User;
using JobFlowProject.Domain.Enums;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JobFlowProject.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Business.Services.CompaneisService;



public class CompanyFeatureService : ICompanyFeatureService
{
    private readonly ICompanyFeatureRepository _companyFeatureRepository;
    private readonly IFeatureRepository _featureRepository;
    private readonly INotificationService _notificationService;
    private readonly IEmailService _emailService;
    private readonly UserManager<AppUser> _userManager;
    private readonly JobFlowDbContext _context;

    public CompanyFeatureService(
        ICompanyFeatureRepository companyFeatureRepository,
        IFeatureRepository featureRepository,
        INotificationService notificationService,
        IEmailService emailService,
        UserManager<AppUser> userManager,
        JobFlowDbContext context)
    {
        _companyFeatureRepository = companyFeatureRepository;
        _featureRepository = featureRepository;
        _notificationService = notificationService;
        _emailService = emailService;
        _userManager = userManager;
        _context = context;
    }

    private async Task NotifyCompanyOwnersAsync(Guid companyId, string title, string message, NotificationTypeEnum type, Func<string, string>? emailTemplate = null)
    {
        var owners = await _context.Users
            .Where(u => u.CompanyId == companyId)
            .ToListAsync();

        foreach (var owner in owners)
        {
            await _notificationService.NotifyAsync(owner.Id, title, message, type, companyId);

            if (!string.IsNullOrWhiteSpace(owner.Email))
            {
                try
                {
                    var body = emailTemplate is null ? message : emailTemplate(owner.Email);
                    await _emailService.SendAsync(owner.Email, title, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Company owner email failed: {ex.Message}");
                }
            }
        }
    }

    public async Task AssignFeatureToCompanyAsync(AssignFeatureToCompanyDto dto)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(x => x.Id == dto.CompanyId && !x.IsDeleted);

        if (company is null)
            throw new ItemNotFoundException("Company not found.");

        var feature = await _featureRepository.GetByIdAsync(dto.FeatureId);

        if (feature is null)
            throw new ItemNotFoundException("Feature not found.");

        var existing = await _companyFeatureRepository
            .GetCompanyFeaturesAsync(dto.CompanyId);

        if (existing.Any(x => x.FeatureId == dto.FeatureId))
            throw new InvalidOperationException("این فیچر قبلاً برای این شرکت تخصیص داده شده است.");

        var companyFeature = new CompanyFeature(
            dto.CompanyId,
            dto.FeatureId,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(feature.DurationDays));

        await _companyFeatureRepository.AddAsync(companyFeature);

        await NotifyCompanyOwnersAsync(
            dto.CompanyId,
            "فیچر جدید تخصیص یافت",
            $"فیچر «{feature.Name}» به مدت {feature.DurationDays} روز به شرکت تخصیص داده شد.",
            NotificationTypeEnum.FeatureAssigned,
            emailTemplate: _ => EmailTemplates.FeatureAssigned(feature.Name, feature.DurationDays));
    }

    public async Task ExtendCompanyFeatureAsync(
        Guid companyFeatureId,
        DateTime newEndDate,
        Guid requesterId)
    {
        var companyFeature = await _companyFeatureRepository
            .GetByIdAsync(companyFeatureId, tracking: true);

        if (companyFeature is null)
            throw new Exception("Company feature not found.");

        companyFeature.Extend(newEndDate);
        companyFeature.SetModificationInfo(requesterId);

        await _companyFeatureRepository.UpdateAsync(companyFeature);

        await NotifyCompanyOwnersAsync(
            companyFeature.CompanyId,
            "تمدید فیچر",
            $"اعتبار فیچر تا تاریخ {newEndDate:yyyy/MM/dd} تمدید شد.",
            NotificationTypeEnum.FeatureAssigned);
    }

    public async Task RemoveFeatureFromCompanyAsync(Guid companyFeatureId, Guid requesterId)
    {
        var companyFeature = await _context.CompanyFeatures
            .Include(x => x.Feature)
            .FirstOrDefaultAsync(x => x.Id == companyFeatureId && !x.IsDeleted);

        if (companyFeature is null)
            throw new Exception("Company feature not found.");

        await _companyFeatureRepository
            .SoftDeleteAsync(companyFeatureId, requesterId);

        await NotifyCompanyOwnersAsync(
            companyFeature.CompanyId,
            "حذف فیچر",
            $"فیچر «{companyFeature.Feature.Name}» از شرکت حذف شد.",
            NotificationTypeEnum.FeatureAssigned);
    }


    public async Task<List<FeatureListDto>> GetCompanyFeaturesAsync(Guid companyId)
    {
        var companyFeatures = await _companyFeatureRepository
            .GetCompanyFeaturesAsync(companyId);

        return companyFeatures.Select(x => new FeatureListDto(
            x.Feature.Id,
            x.Feature.Name,
            x.Feature.Description,
            x.Feature.Price,
            x.Feature.DurationDays,
            x.IsActive && x.EndDate > DateTime.UtcNow,
            x.Feature.FeatureType
        )).ToList();
    }

    public async Task<List<CompanyFeatureDashboardDto>> GetCompanyFeatureDashboardAsync(Guid companyId)
    {
        var companyFeatures = await _companyFeatureRepository
            .GetAllCompanyFeaturesAsync(companyId);

        var now = DateTime.UtcNow;

        return companyFeatures.Select(x => new CompanyFeatureDashboardDto(
            x.Id,
            x.FeatureId,
            x.Feature.Name,
            x.Feature.Description,
            x.Feature.FeatureType,
            x.Feature.Price,
            x.StartDate,
            x.EndDate,
            x.IsActive && x.EndDate > now,
            x.DaysRemaining
        )).ToList();
    }

}
