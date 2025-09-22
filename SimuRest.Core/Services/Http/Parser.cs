using System.Net;
using SimuRest.Core.Models;

namespace SimuRest.Core.Services.Http;

public class Parser
{
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
            return new SimuRequest(HttpMethod.Parse(ctx.HttpMethod), path);
        }

        catch
        {
            Console.WriteLine("Oops, Something went wrong parsing the Http method.");
            return null;
        }
    }
}