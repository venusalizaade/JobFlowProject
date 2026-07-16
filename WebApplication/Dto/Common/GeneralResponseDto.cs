using WebApplication1.Dto.Authentication;

namespace WebApplication1.Dto.Authentication;

public class GeneralResponseDto : BaseResponseDto<GeneralDto>
{
    public GeneralResponseDto(Guid resourceId)
        : base(new GeneralDto(resourceId))
    {
    }

    public GeneralResponseDto(string message, string code)
        : base(message, code)
    {
    }
}