using MediatR;
using Microsoft.AspNetCore.Http;

namespace HelloStory.Shared.BLL.Interfaces;

public interface IHttpRequestHandler<T> : IRequestHandler<T, IResult> where T : IHttpRequest
{
}