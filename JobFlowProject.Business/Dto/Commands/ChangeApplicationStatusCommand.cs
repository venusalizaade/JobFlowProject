using JobFlowProject.Domain.Enums;

namespace JobFlowProject.Business.Dto.Commands;

public record ChangeApplicationStatusCommand(
    Guid JobApplicationId,
    JobApplicationStatusEnum Status);