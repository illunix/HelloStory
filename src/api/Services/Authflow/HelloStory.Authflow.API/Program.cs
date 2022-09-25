using HelloStory.Authflow.API.Extensions;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services
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