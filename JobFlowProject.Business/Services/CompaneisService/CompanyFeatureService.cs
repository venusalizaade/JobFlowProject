using JobFlowProject.Business.Dto.Feature;
using JobFlowProject.Business.Exceptions.BaseExeption;
using JobFlowProject.Business.Interfaces.User;
using JobFlowProject.Domain.Entities.Componies.ComponyFeatures;
using JobFlowProject.Domain.Interfaces.Repository;
using JobFlowProject.Infrastructure.DbContext.AppDbContext;
using JobFlowProject.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JobFlowProject.Business.Services.CompaneisService;



public class CompanyFeatureService : ICompanyFeatureService
{
    private readonly ICompanyFeatureRepository _companyFeatureRepository;
    private readonly IFeatureRepository _featureRepository;
    private readonly JobFlowDbContext _context;

    public CompanyFeatureService(
        ICompanyFeatureRepository companyFeatureRepository,
        IFeatureRepository featureRepository,
        JobFlowDbContext context)
    {
        _companyFeatureRepository = companyFeatureRepository;
        _featureRepository = featureRepository;
        _context = context;
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
    }

    public async Task RemoveFeatureFromCompanyAsync(Guid companyFeatureId, Guid requesterId)
    {
        var companyFeature = await _companyFeatureRepository
            .GetByIdAsync(companyFeatureId);

        if (companyFeature is null)
            throw new Exception("Company feature not found.");

        await _companyFeatureRepository
            .SoftDeleteAsync(companyFeatureId, requesterId);
    }


    public async Task<List<FeatureListDto>> GetCompanyFeaturesAsync(Guid companyId)
    {
        var companyFeatures = await _companyFeatureRepository
            .GetCompanyFeaturesAsync(companyId);

        return companyFeatures.Select(x => new FeatureListDto(
            x.Feature.Id,
            x.Feature.Name,
            x.Feature.Price,
            x.Feature.DurationDays
        )).ToList();
    }

  


}