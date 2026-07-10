using JobFlowProject.Domain.Entites.User;

namespace JobFlowProject.Domain.Entites.Componyes.ComponyFeatures;


public class CompanyFeature : BaseEntity
{
    /// <summary>
    /// آیدی شرکت
    /// </summary>
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }

    /// <summary>
    /// آیدی فیچر
    /// </summary>
    public Guid FeatureId { get; set; }
    public Feature Feature { get; set; }

    /// <summary>
    /// تاریخ شروع اعتبار
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// تاریخ انقضای اعتبار
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// آیا هنوز فعال است؟
    /// </summary>
    public bool IsActive => DateTime.UtcNow <= EndDate;

    public override void Validation()
    {
        if (EndDate <= StartDate)
            throw new Exception("EndDate must be after StartDate");
    }
}