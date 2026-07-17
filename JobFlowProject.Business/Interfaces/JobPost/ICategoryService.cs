using JobFlowProject.Business.Dto.CompanyDto;

namespace JobFlowProject.Business.Interfaces.EmployerInterfaces;

public interface ICategoryService
{
    Task<List<CategoryResponseDto>> GetAllAsync();
}