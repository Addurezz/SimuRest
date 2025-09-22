using System.Net;
using SimuRest.Core.Models;

namespace SimuRest.Core.Services.Http;

public class RequestHandler
{
    private readonly ResponseWriter _writer;
    
    private readonly Router _router;
    
    private readonly Parser _parser;

    public RequestHandler(Router router, Parser parser, ResponseWriter writer)
    {
        _writer = writer;
        _router = router;
        _parser = parser;
    }

    public void Handle(HttpListenerContext ctx)
    {
        SimuRequest req = _parser.Parse(ctx);
        SimuResponse response = req != null ? _router.Handle(req) : null;
        _writer.Write(ctx, response);
    }
}