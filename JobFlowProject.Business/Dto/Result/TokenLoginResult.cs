namespace JobFlowProject.Business.Dto.Token;

public record TokenLoginResult(
    string AccessToken,
    string RefreshToken,
    double ExpiresInSeconds);