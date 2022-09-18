namespace HumanExpBook.BLL.Interfaces;

public interface IHttpRequestHandler<T> : IRequestHandler<T, IResult> where T : IHttpRequest
{
}
