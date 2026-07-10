using JobFlowProject.Domain.Entites.User;
using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Domain.Entites.Componyes.ComponyFeatures;


public class Payment : BaseEntity


{
    private Payment() { }
    public Payment(decimal amount, Guid companyId, Guid? featureId = null)
    {
        Amount = amount;
        CompanyId = companyId;
        FeatureId = featureId;
        VerifiedAt = DateTime.UtcNow;
        Status = PaymentStatusEnum.Pending;
    }
    
    /// <summary>
    /// آیدی تراکنش
    /// </summary>
   public Guid TransactionId { get; private set; }
    /// <summary>
    /// مبلغ پرداخت‌شده به تومان
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// تاریخ و زمان انجام پرداخت
    /// </summary>
    public DateTime? VerifiedAt { get; private set; } 
    

    /// <summary>
    /// وضعیت پرداخت 
    /// </summary>
    public PaymentStatusEnum Status { get; private set; } 

    /// <summary>
    /// آیدی شرکت پرداخت‌کننده
    /// </summary>
    public Guid CompanyId { get;  set; }

    /// <summary>
    /// شرکت پرداخت‌کننده
    /// </summary>
    public Company Company { get; private set; }

    /// <summary>
    /// آیدی فیچر خریده شده 
    /// </summary>
    public Guid? FeatureId { get; set; }

    /// <summary>
   ///فیچر خریداری شده
    /// </summary>
    public Feature? Feature { get; private set; }
    

    public override void Validation()
    {
        if (Amount <= 0)
            throw new Exception("Amount must be greater than zero");
        
    }
}