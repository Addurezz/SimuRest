using System.Net;
using System.Text;
using SimuRest.Core.Models;

namespace SimuRest.Core.Services;

public class ResponseWriter
{
    public void Write(HttpListenerContext ctx, SimuResponse response)
    {
        if (response is null) return;
        
        int status = response.StatusCode;
        string body = response.Body;

        ctx.Response.StatusCode = status;
        byte[] buffer = Encoding.UTF8.GetBytes(body);

        ctx.Response.ContentType = "text/plain";
        ctx.Response.ContentLength64 = buffer.Length;
        ctx.Response.OutputStream.Write(buffer,0,buffer.Length);
        ctx.Response.OutputStream.Close();
    }
}