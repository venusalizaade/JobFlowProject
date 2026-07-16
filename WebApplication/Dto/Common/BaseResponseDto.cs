namespace WebApplication1.Dto.Authentication;

public class BaseResponseDto<T>
{
    public bool IsSuccess { get; set; }

    public T? Data { get; set; }

    public BaseErrorDto? Error { get; set; }

    public BaseResponseDto(T data)
    {
        IsSuccess = true;
        Data = data;
    }

    public BaseResponseDto(string message, string code)
    {
        IsSuccess = false;

        Error = new BaseErrorDto
        {
            Message = message,
            Code = code
        };
    }
}

public class BaseErrorDto
{
    public string Message { get; set; }

    public string Code { get; set; }
}