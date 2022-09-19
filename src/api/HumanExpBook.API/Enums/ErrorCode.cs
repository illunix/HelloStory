namespace HumanExpBook.API.Enums;

public enum ErrorCode
{
    General = 1,
    EntityNotFound,
    EntityWithSamePropertyValueAlreadyExist,
    InvalidCredentials,
    InvalidToken,
    ExpiredRefreshToken,
    UserWithThisEmailAlreadyExist
}