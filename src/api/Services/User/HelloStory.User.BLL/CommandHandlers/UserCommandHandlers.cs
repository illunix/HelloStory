using HelloStory.Shared.DAL.Context;
using HelloStory.Shared.DAL.Enities;
using HelloStory.Shared.BLL.Exceptions;
using HelloStory.Shared.BLL.Interfaces;
using HelloStory.User.BLL.Commands;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace HelloStory.User.BLL.CommandHandlers;

public sealed partial class UserCommandHandlers :
    IHttpRequestHandler<SignUpCommand>
{
    private readonly HelloStoryContext _ctx;

    [HttpPost]
    public async Task<IResult> Handle(
        SignUpCommand req,
        CancellationToken ct
    )
    {
        if (await _ctx.Users.AnyAsync(q => q.Email == req.Email))
            throw new EntityWithSamePropertyValueAlreadyExistException(
                nameof(UserEntity),
                nameof(UserEntity.Email)
            );

        if (await _ctx.Users.AnyAsync(q => q.Username == req.Username))
            throw new EntityWithSamePropertyValueAlreadyExistException(
                nameof(UserEntity),
                nameof(UserEntity.Username)
            );

        var bytes = () =>
        {
            using var randomNumberGenerator = new RNGCryptoServiceProvider();

            var salt = new byte[32];
            randomNumberGenerator.GetBytes(salt);

            return salt;
        };

        var hashedPassword = () =>
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    req.Password,
                    bytes(),
                    KeyDerivationPrf.HMACSHA256,
                    10000,
                    256 / 8
            ));
        };

        _ctx.Add(new UserEntity(
            req.Email,
            req.Username,
            hashedPassword(),
            Convert.ToBase64String(bytes())
        ));

        await _ctx.SaveChangesAsync();

        return Results.Ok();
    }
}