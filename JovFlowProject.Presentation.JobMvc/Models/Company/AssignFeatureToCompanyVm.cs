using System.ComponentModel.DataAnnotations;
using JobFlowProject.Business.Dto.Feature;

namespace JovFlowProject.JobMvc.Models.Company;

public class AssignFeatureToCompanyVm
{
    [Required]
    public Guid CompanyId { get; set; }

    [Required]
    public Guid FeatureId { get; set; }

    public AssignFeatureToCompanyDto ToDto()
    {
        return new AssignFeatureToCompanyDto(
            CompanyId,
            FeatureId
        );
    }
}