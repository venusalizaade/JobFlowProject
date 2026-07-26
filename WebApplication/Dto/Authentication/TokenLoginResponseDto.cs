using JobFlowProject.Business.Dto.Token;

namespace WebApplication1.Dto.Authentication;



    public class TokenLoginResponseDto : BaseResponseDto<TokenDto>
    {
        public TokenLoginResponseDto(TokenDto data)
            : base(data)
        {
        }

        public static TokenLoginResponseDto FromResult(TokenLoginResult result)
        {
            return new TokenLoginResponseDto(
                new TokenDto(
                    result.AccessToken,
                    result.ExpiresInSeconds ));
        }
    
}