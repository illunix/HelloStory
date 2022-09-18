using HumanExpBook.API.Enums;
using HumanExpBook.BLL.Exceptions;
using HumanExpBook.BLL.Exceptions.Auth;
using System.Net;

namespace HumanExpBook.API.Extensions;

internal static class ExceptionFilterExtensions
{
    public static (
        HttpStatusCode statusCode,
        ErrorCode errorCode
    ) ParseException(this Exception exception)
    {
        return exception switch
        {
            NotFoundException _ => (
                HttpStatusCode.NotFound,
                ErrorCode.NotFound
            ),
            InvalidCredentialsException _ => (
                HttpStatusCode.Unauthorized, 
                ErrorCode.InvalidCredentials
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                ErrorCode.General
            ),
        };
    }
}