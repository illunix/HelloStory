namespace HelloStory.Authflow.BLL.Exceptions;

public sealed class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base($"Invalid username or password.") { }
}