namespace HumanExpBook.BLL.Exceptions.Authflow;

public sealed class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base($"Invalid username or password.") { }
}