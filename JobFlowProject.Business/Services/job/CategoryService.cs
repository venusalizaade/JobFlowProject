using JobFlowProject.Business.Dto.CompanyDto;
using JobFlowProject.Business.Interfaces.EmployerInterfaces;
using JobFlowProject.Domain.Interfaces.Repository;

namespace JobFlowProject.Business.Services.job;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();

        return categories
            .Select(c => new CategoryResponseDto(
                c.Id,
                c.Name,
                c.Description))
            .ToList();
    }
}