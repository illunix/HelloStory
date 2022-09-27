namespace HelloStory.Authflow.BLL.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(Guid userId);
    string GenerateRefreshToken();
}
