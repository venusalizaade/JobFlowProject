namespace WebApplication1.Dto.Authentication;

public record TokenDto(
    string AccessToken,
    double ExpiresIn);