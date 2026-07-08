using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.Job;


public class Payment : BaseEntity
{
    /// <summary>
    /// مبلغ پرداخت‌شده به تومان
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// تاریخ و زمان انجام پرداخت
    /// </summary>
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    

    /// <summary>
    /// وضعیت پرداخت 
    /// </summary>
    public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Success;

    /// <summary>
    /// آیدی شرکت پرداخت‌کننده
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// شرکت پرداخت‌کننده
    /// </summary>
    public Company Company { get; set; }

    /// <summary>
    /// آیدی فیچر خریده شده 
    /// </summary>
    public Guid? FeatureId { get; set; }

    /// <summary>
   ///فیچر خریداری شده
    /// </summary>
    public Feature? Feature { get; set; }
    

    public override void Validation()
    {
        if (Amount <= 0)
            throw new Exception("Amount must be greater than zero");
        
    }
}