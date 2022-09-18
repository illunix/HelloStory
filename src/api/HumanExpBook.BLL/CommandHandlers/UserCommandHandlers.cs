using HumanExpBook.BLL.Commands.User;
using HumanExpBook.BLL.Exceptions;
using HumanExpBook.BLL.Interfaces;
using HumanExpBook.Common.Security;
using HumanExpBook.DAL.Context;
using HumanExpBook.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanExpBook.BLL.CommandHandlers;

public sealed partial class UserCommandHandlers :
    IHttpRequestHandler<SignUpCommand>
{
    private readonly InternalDbContext _ctx;

    [HttpGet]
    public async Task<IResult> Handle(
        SignUpCommand req,
        CancellationToken ct
    )
    {
        if (await _ctx.Users.AnyAsync(q => q.Email == req.Email))
            throw new EntityWithSamePropertyValueAlreadyExistException(
                nameof(User),
                nameof(User.Email)
            );

        var salt = SecurityHelper.GetRandomBytes();

        _ctx.Add(new User
        {
            Email = req.Email,
            Username = req.Username,
            Password = SecurityHelper.HashPassword(
                req.Password,
                salt
            ),
            Salt = Convert.ToBase64String(salt)
        });

        await _ctx.SaveChangesAsync();

        return Results.Ok();
    }
}