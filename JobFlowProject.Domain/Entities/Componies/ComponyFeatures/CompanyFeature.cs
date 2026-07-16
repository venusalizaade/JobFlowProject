using JobFlowProject.Domain.Entites;

namespace JobFlowProject.Domain.Entities.Componies.ComponyFeatures;


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

    public override void Validate()
    {
        if (EndDate <= StartDate)
            throw new Exception("EndDate must be after StartDate");
        
        if (StartDate < DateTime.UtcNow.Date)
            throw new Exception("StartDate cannot be in the past");
        
        if (EndDate < DateTime.UtcNow.Date)
            throw new Exception("EndDate cannot be in the past");

    }
}