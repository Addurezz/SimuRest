using System.Net;

namespace SimuRest.Core.Services.Http;

public interface IHttpContext
{
    public HttpListenerContext Context { get; }
    public HttpListenerResponse Response { get; }
    public Uri? Url { get; }
    public string HttpMethod { get; }
}