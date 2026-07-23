namespace JobFlowProject.Business.Dto.User;

public record AttachmentFileResponseDto(
    string FileName,
    string FilePath,
    string FileType);