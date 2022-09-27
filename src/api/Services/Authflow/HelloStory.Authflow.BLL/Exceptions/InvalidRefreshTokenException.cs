﻿namespace HelloStory.Authflow.BLL.Exceptions;

public sealed class InvalidRefreshTokenException : Exception
{
    public InvalidRefreshTokenException() : base($"Invalid refresh token.") { }
}