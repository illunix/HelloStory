using FluentValidation;
using HelloStory.Posts.BLL.Commands;
using HelloStory.Shared.DAL.Context;
using HelloStory.Posts.API.Extensions;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services
    .AddDbContext<HelloStoryContext>(q => q.UseNpgsql(configuration["dbConnectionString"]!))
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddMediatR(
        q => q.AsScoped(),
        typeof(CreatePostCommand)
    )
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

#if DEBUG
app
    .UseSwagger()
    .UseSwaggerUI();
#endif

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();