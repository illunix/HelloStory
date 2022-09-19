namespace HumanExpBook.BLL.Exceptions.Authflow;

public sealed class RefreshTokenAlreadyRevoked : Exception
{
    public RefreshTokenAlreadyRevoked() : base($"Refresh token was already revoked.") { }
}