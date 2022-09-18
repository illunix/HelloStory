using HumanExpBook.BLL.Commands.Authflow;
using HumanExpBook.BLL.Exceptions.Auth;
using HumanExpBook.BLL.Exceptions;
using HumanExpBook.BLL.Interfaces;
using HumanExpBook.Common.Security;
using HumanExpBook.DAL.Context;
using HumanExpBook.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HumanExpBook.BLL.Services;
using HumanExpBook.Common.DTO.Authflow;
using Microsoft.Extensions.Caching.Memory;
using Enyim.Caching;
using HumanExpBook.Common.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HumanExpBook.BLL.CommandHandlers;

public sealed partial class AuthflowCommandHandlers :
    IHttpRequestHandler<SignInCommand>,
    IHttpRequestHandler<RefreshAccessTokenCommand>,
    IHttpRequestHandler<RevokeRefreshTokenCommand>
{
    private readonly InternalDbContext _ctx;
    private readonly ITokenService _tokenService;
    private readonly IMemcachedClient _cache;
    private readonly IOptions<JwtOptions> _options;

    [HttpPost]
    public async Task<IResult> Handle(
        SignInCommand req,
        CancellationToken ct
    )
    {
        var user = await _ctx.Users
            .Where(q =>
                q.Email == req.EmailOrUsername ||
                q.Username == req.EmailOrUsername
            )
            .Select(q => new {
                q.Id,
                q.Password,
                q.Salt
            })
            .FirstOrDefaultAsync();
        if (
            user is null ||
            !SecurityHelper.ValidatePassword(
                req.Password,
                user.Password!,
                user.Salt!
            )
        )
            throw new InvalidCredentialsException();

        await _cache.AddAsync(
            user.Id.ToString(),
            _tokenService.GenerateRefreshToken(),
            (int)_options.Value.ValidFor.TotalSeconds
        );

        return Results.Ok(new AccessTokenDTO(_tokenService.GenerateAccessToken(user.Id.ToString())));
    }

    [HttpPost("token/refresh")]
    public async Task<IResult> Handle(
        RefreshAccessTokenCommand req,
        CancellationToken ct
    )
    {
        var refreshToken = await _cache.GetAsync(req.CurrentUserId);
        if (refreshToken is null)
            throw new InvalidRefreshTokenException();

        _cache.Add(
            req.CurrentUserId,
            _tokenService.GenerateRefreshToken(),
            (int)_options.Value.ValidFor.TotalSeconds
        );

        return Results.Ok(req.CurrentUserId);
    }

    [HttpPost("token/revoke")]
    public async Task<IResult> Handle(
        RevokeRefreshTokenCommand req,
        CancellationToken ct
    )
    {
        var refreshToken = await _cache.GetAsync(req.CurrentUserId);
        if (refreshToken is null)
            throw new InvalidRefreshTokenException();

        await _cache.RemoveAsync(req.CurrentUserId);

        return Results.Ok();
    }
}