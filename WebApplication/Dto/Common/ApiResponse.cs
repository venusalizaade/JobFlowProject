namespace WebApplication1.Dto.Authentication;

public record ApiResponse<T>(bool Success, T? Data, string? Message
);