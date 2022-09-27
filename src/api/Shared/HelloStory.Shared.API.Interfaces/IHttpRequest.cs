using MediatR;
using Microsoft.AspNetCore.Http;

namespace HelloStory.Shared.BLL.Interfaces;

public interface IHttpRequest : IRequest<IResult>
{ 
}