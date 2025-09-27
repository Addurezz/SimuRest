using System.Net;

namespace SimuRest.Core.Services.Http;

/// <summary>
/// Wrapper class for the <see cref="HttpListenerContext"/>.
/// </summary>
public interface IHttpContext
{
    /// <summary>
    /// Gets the <see cref="HttpListenerContext"/>.
    /// </summary>
    public HttpListenerContext Context { get; }
    
    /// <summary>
    /// Gets the response from the <see cref="HttpListenerContext"/>.
    /// </summary>
    public HttpListenerResponse Response { get; }
    
    /// <summary>
    /// Gets the Url from the <see cref="HttpListenerContext"/>.
    /// </summary>
    public Uri? Url { get; }
    
    /// <summary>
    /// Gets the <see cref="HttpMethod"/> from the <see cref="HttpListenerContext"/>.
    /// </summary>
    public string HttpMethod { get; }
    
    /// <summary>
    /// Gets the content from the <see cref="HttpListenerContext"/>.
    /// </summary>
    public string Body { get; }
}