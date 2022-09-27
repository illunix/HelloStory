using FluentValidation;
using HelloStory.Authflow.BLL.Commands;
using HelloStory.Shared.DAL.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HelloStory.Authflow.API.Extensions;
using HelloStory.Authflow.BLL.Services;
using HelloStory.Authflow.BLL.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddDbContext<HelloStoryContext>(q => q.UseNpgsql(configuration["dbConnectionString"]!))
    .AddEnyimMemcached()
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddMediatR(
        q => q.AsScoped(),
        typeof(SignInCommand)
    )
    .AddSingleton<ITokenService, TokenService>()
#if DEBUG
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();
#endif
#if RELEASE
    .AddAWSLambdaHosting(LambdaEventSource.HttpApi);
#endif  

builder.Logging
    .ClearProviders()
    .AddSerilog(new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger()
    );

var app = builder.Build();

app.UseEnyimMemcached();

#if DEBUG
app
    .UseSwagger()
    .UseSwaggerUI();
#endif

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();