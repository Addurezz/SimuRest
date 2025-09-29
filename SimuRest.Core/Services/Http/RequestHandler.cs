using System.Net;
using SimuRest.Core.Models;
using SimuRest.Core.Services.Perseverance;

namespace SimuRest.Core.Services.Http;

/// <summary>
/// Processes all incoming requests.
/// </summary>
public class RequestHandler
{
    /// <summary>
    /// Processes the <see cref="HttpListenerContext"/> and does all the required steps to write a response into the <see cref="HttpListenerResponse"/> as an asynchronous operation.
    /// </summary>
    /// <param name="ctx">The <see cref="HttpListenerContext"/> to process.</param>
    /// <param name="provider">The <see cref="ServiceProvider"/> that stores the services to use.</param>
    public async Task Handle(HttpListenerContext ctx, ServiceProvider provider)
    {
        try
        {
            HttpContextAdapter context = new HttpContextAdapter(ctx);
            Parser parser = provider.GetService<Parser>();
            Router router = provider.GetService<Router>();
            ResponseWriter writer = provider.GetService<ResponseWriter>();
            
            SimuRequest? req = _GetSimuRequestFromContext(parser, context);
            SimuResponse response = await _GetSimuResponseFromRouter(router, req);
            await _WriteToStream(writer, context, response);
        }
        catch (Exception e)
        {
            await _WriteToStream(
                new ResponseWriter(), 
                new HttpContextAdapter(ctx),
                new SimuResponse(500, $"Internal server error: {e.Message}"));
        }
    }
    
    private SimuRequest? _GetSimuRequestFromContext(Parser parser, IHttpContext ctx)
    {
        return parser.Parse(ctx);
    }
    
    private async Task<SimuResponse> _GetSimuResponseFromRouter(Router router, SimuRequest? req)
    {
        if (req is null) throw new ArgumentNullException();
        return await router.Handle(req);
    }
    
    private async Task _WriteToStream(ResponseWriter writer, IHttpContext ctx, SimuResponse response)
    {
        await writer.Write(ctx, response);
    }
}