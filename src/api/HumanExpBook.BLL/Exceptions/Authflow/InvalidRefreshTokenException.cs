namespace HumanExpBook.BLL.Exceptions.Authflow;

public sealed class InvalidRefreshTokenException : Exception
{
    public InvalidRefreshTokenException() : base($"Invalid refresh token.") { }
}