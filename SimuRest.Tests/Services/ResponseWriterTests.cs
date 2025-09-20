using System.Net;
using SimuRest.Core.Models;
using SimuRest.Core.Services;

namespace SimuRest.Tests.Services;

public class ResponseWriterTests
{
    [Fact]
    public void WriteTest()
    {
        var port = 5000;
        var listener = new HttpListener();
        listener.Prefixes.Add($"http://localhost:{port}/");
        listener.Start();
        
        var responseWriter = new ResponseWriter();

        var context = listener.GetContext();
        var response = new SimuResponse(200, "<h1>Hello World!</h1>");
        responseWriter.Write(context,response);
    }
}