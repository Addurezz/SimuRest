using System.Net;
using System.Text;
using SimuRest.Core.Models;

namespace SimuRest.Core.Services.Http;

/// <summary>
/// Provides the ability to write to the <see cref="HttpListenerResponse"/>.
/// </summary>
public class ResponseWriter
{
    /// <summary>
    /// Writes to the <see cref="HttpListenerResponse"/> file stream as asynchronous operation.
    /// </summary>
    /// <param name="ctx">The <see cref="HttpListenerResponse"/> to be written to.</param>
    /// <param name="response">The <see cref="SimuResponse"/> to write to the<see cref="HttpListenerResponse"/>.</param>
    public async Task Write(IHttpContext ctx, SimuResponse? response)
    {
        if (response is null)
            response = new SimuResponse(404, "Not found");
        int status = response.StatusCode;
        string body = response.Body;

        ctx.Response.StatusCode = status;
        byte[] buffer = Encoding.UTF8.GetBytes(body);

        ctx.Response.ContentType = "text/plain";
        ctx.Response.ContentLength64 = buffer.Length;
        await ctx.Response.OutputStream.WriteAsync(buffer,0,buffer.Length);
        ctx.Response.OutputStream.Close();
    }
}