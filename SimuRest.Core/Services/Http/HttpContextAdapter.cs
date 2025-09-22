using System.Net;

namespace SimuRest.Core.Services.Http;

public class HttpContextAdapter : IHttpContext
{
    public HttpContextAdapter(HttpListenerContext ctx)
    {
        Context = ctx;
    }

    public HttpListenerContext Context { get; }
    public HttpListenerResponse Response => Context.Response;
    public Uri? Url => Context.Request.Url;

    public string HttpMethod => Context.Request.HttpMethod;
}