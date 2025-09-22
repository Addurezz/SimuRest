using System.Net;
using SimuRest.Core.Models;

namespace SimuRest.Core.Services.Http;

public class Parser
{
    public SimuRequest Parse(HttpListenerContext ctx)
    {
        try
        {
            string path = ctx.Request.Url.AbsolutePath;
            if (String.IsNullOrWhiteSpace(path))
                path = "/";
            
            HttpMethod method;
            switch (ctx.Request.HttpMethod)
            {
                case "GET":
                    method = HttpMethod.Get;
                    break;
                case "POST":
                    method = HttpMethod.Post;
                    break;
                default:
                    throw new Exception();
            }

            return new SimuRequest(method, path);
        }

        catch
        {
            Console.WriteLine("Oops, Something went wrong here.");
            return null;
        }
    }
}