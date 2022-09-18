using HumanExpBook.API.Extensions;
using HumanExpBook.BLL.Interfaces;
using HumanExpBook.BLL.Services;
using HumanExpBook.Common.Options;
using HumanExpBook.DAL.Context;
using HumanExpBook.DAL.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Riok.Mapperly.Abstractions;
using Serilog;
using System.Text;
using HumanExpBook.BLL.Commands.User;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["jwt:secretKey"]!));

builder.Services
    .AddDbContext<InternalDbContext>(q => q.UseNpgsql(configuration["dbConnectionString"]))
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddMediatR(
        q => q.AsScoped(),
        typeof(SignUpCommand)
    )
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddMappers()
    .AddDefaultAWSOptions(configuration.GetAWSOptions())
#if RELEASE
    .AddAWSLambdaHosting(LambdaEventSource.HttpApi);
#endif
    .AddEnyimMemcached()
    .Configure<JwtOptions>(q =>
    {
        q.Issuer = configuration["jwt:issuer"];
        q.Audience = configuration["jwt:audience"];
        q.SigningCredentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256
        );
    })
    .AddSingleton<ITokenService, TokenService>()
    .AddAuthorization()
    .AddAuthentication(q =>
    {
        q.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        q.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(q =>
    {
        q.ClaimsIssuer = configuration["jwt:issuer"];
        q.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["jwt:issuer"],

            ValidateAudience = true,
            ValidAudience = configuration["jwt:audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,

            RequireExpirationTime = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        q.SaveToken = true;

        q.Events = new()
        {
            OnAuthenticationFailed = ctx =>
            {
                if (ctx.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    ctx.Response.Headers.Add(
                        "Token-Expired",
                        "true"
                    );
                }

                return Task.CompletedTask;
            }
        };
    });


builder.Logging
    .ClearProviders()
    .AddSerilog(new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger()
    );

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app
    .UseAuthentication()
    .UseAuthorization();

app.UseEnyimMemcached();

app.MapEndpoints();

app.UseExceptionHandler(q =>
{
    q.Run(async ctx =>
    {
        var exceptionHandlerPathFeature = ctx.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature!.Error;

        var (
            statusCode,
            errorCode
        ) = exception.ParseException();

        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)statusCode;

        await ctx.Response.WriteAsJsonAsync(new
        {
            error = exception.Message,
            code = errorCode
        });
    });
});

app.Run();