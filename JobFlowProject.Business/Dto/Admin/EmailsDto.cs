namespace JobFlowProject.Business.Dto.Admin;

public record EmailSettingDto(
    string Host,
    int Port,
    bool EnableSsl,
    string Username,
    string SenderName,
    string SenderEmail
);

public record UpdateEmailSettingDto(
    string Host,
    int Port,
    bool EnableSsl,
    string Username,
    string Password,
    string SenderName,
    string SenderEmail
    );