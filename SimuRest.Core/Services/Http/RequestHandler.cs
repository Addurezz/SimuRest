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

    public async Task Handle(HttpListenerContext ctx)
    {
        HttpContextAdapter adapter = new HttpContextAdapter(ctx);
        SimuRequest? req = GetSimuRequestFromContext(adapter);
        SimuResponse response = await GetSimuResponseFromRouter(req);
        
        await WriteToStream(adapter, response);
    }

    public SimuRequest? GetSimuRequestFromContext(IHttpContext ctx)
    {
        return _parser.Parse(ctx);
    }

    public async Task<SimuResponse> GetSimuResponseFromRouter(SimuRequest? req)
    {
        return await _router.Handle(req);
    }

    public async Task WriteToStream(IHttpContext ctx, SimuResponse response)
    {
        await _writer.Write(ctx, response);
    }
}