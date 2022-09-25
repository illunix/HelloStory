using Enyim.Caching;
using HelloStory.Authflow.BLL.Commands;
using HelloStory.Authflow.BLL.Exceptions;
using HelloStory.Authflow.BLL.Interfaces;
using HelloStory.Authflow.BLL.Services;
using HelloStory.Authflow.Common.DTO;
using HelloStory.Authflow.Common.Options;
using HelloStory.DAL.Context;
using HelloStory.Shared.BLL.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Authentication;
using System.Text;

namespace HelloStory.Authflow.BLL.CommandHandlers;

public sealed partial class AuthflowCommandHandlers : 
    IHttpRequestHandler<SignInCommand>,
    IHttpRequestHandler<RefreshAccessTokenCommand>,
    IHttpRequestHandler<RevokeRefreshTokenCommand>
{
    private readonly HelloStoryContext _ctx;
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
        if  (user is null)
            throw new InvalidCredentialsException();

        var validatePassword = () =>
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                req.Password,
                Encoding.ASCII.GetBytes(user.Salt),
                KeyDerivationPrf.HMACSHA256,
                10000,
                256 / 8
            )) == user.Password;
        };


        if (!validatePassword())
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
        var refreshToken = await _cache.GetAsync(req.CurrentUserId.ToString());
        if (refreshToken is null)
            throw new InvalidRefreshTokenException();

        _cache.Add(
            req.CurrentUserId.ToString(),
            _tokenService.GenerateRefreshToken(),
            (int)_options.Value.ValidFor.TotalSeconds
        );

        return Results.Ok(req.CurrentUserId);
    }

    [HttpDelete("token/revoke")]
    public async Task<IResult> Handle(
        RevokeRefreshTokenCommand req,
        CancellationToken ct
    )
    {
        var refreshToken = await _cache.GetAsync(req.CurrentUserId.ToString());
        if (refreshToken is null)
            throw new InvalidRefreshTokenException();

        await _cache.RemoveAsync(req.CurrentUserId.ToString());

        return Results.Ok();
    }
}