namespace JobFlowProject.Business.Dto.JobPost;


public record CreateJobApplicationDto(
    Guid JobPostId,
    Guid AttachmentId
);