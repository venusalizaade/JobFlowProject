using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.CompanyDto;

namespace JovFlowProject.JobMvc.Models.Company;

public class CreateCategoryVm
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public CreateCategoryDto ToDto()
    {
        return new CreateCategoryDto(
            Name,
            Description
        );
    }
}