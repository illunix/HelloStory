using FluentValidation;
using HelloStory.Authflow.API.Extensions;
using HelloStory.Authflow.BLL.Commands;
using HelloStory.Authflow.BLL.Interfaces;
using HelloStory.Authflow.BLL.Services;
using HelloStory.Authflow.Common.Options;
using HelloStory.DAL.Context;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .Configure<JwtOptions>(q =>
    {
        q.Issuer = configuration["jwt:issuer"];
        q.Audience = configuration["jwt:audience"];
        q.SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["jwt:secretKey"]!)),
            SecurityAlgorithms.HmacSha256
        );
    })
    .AddDbContext<HelloStoryContext>(q => q.UseNpgsql(configuration["dbConnectionString"]!))
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddMediatR(
        q => q.AsScoped(),
        typeof(SignInCommand)
    )
    .AddEnyimMemcached()
    .AddSingleton<ITokenService, TokenService>()
#if DEBUG
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();
#endif
#if RELEASE
    .AddAWSLambdaHosting(LambdaEventSource.HttpApi);
#endif  

var app = builder.Build();

#if DEBUG
app
    .UseSwagger()
    .UseSwaggerUI();
#endif

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();