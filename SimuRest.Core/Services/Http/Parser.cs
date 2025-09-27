using System.Net;
using SimuRest.Core.Models;

namespace SimuRest.Core.Services.Http;

/// <summary>
/// Translates a <see cref="HttpListenerContext"/>.
/// </summary>
public class Parser
{
    /// <summary>
    /// Parses the specified <see cref="IHttpContext"/> into a <see cref="SimuRequest"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="IHttpContext"/> to parse.</param>
    /// <returns>The <see cref="SimuRequest"/>.</returns>
    public SimuRequest? Parse(IHttpContext ctx)
    {
        string? path;

        try
        {
            path = ctx.Url.AbsolutePath;
        }
        catch
        {
            path = null;
            Console.WriteLine("Oops, Something went wrong getting the path.");
        }

        if (String.IsNullOrWhiteSpace(path))
            path = "/";

        try
        {
            return new SimuRequest(HttpMethod.Parse(ctx.HttpMethod), path, ctx.Body);
        }

        catch
        {
            Console.WriteLine("Oops, Something went wrong parsing the Http method.");
            return null;
        }
    }
}