namespace JobFlowProject.Domain.Enums;

public enum NotificationTypeEnum
{
    ResumeReceived = 1,
    ApplicationReviewed = 2,
    ApplicationConfirmed = 3,
    JobPostExpired = 4,
    PaymentRequired = 5,
    PaymentConfirmed = 6,
    EmployerVerificationRequired = 7,
    FeaturePurchaseRequest = 8,
    FeatureAssigned = 9,
    FeatureExpired = 10,
    JobFeatureActivated = 11,
    JobFeatureCancelled = 12,
    ReviewReceived = 13,
    ReviewStatusChanged = 14,
    ReviewReported = 15,
    JobPostVerified = 16,
    System = 17,
    EmployerVerified = 18,
    EmployerRejected = 19
}
