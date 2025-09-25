using System.Net;
using SimuRest.Core.Models;

namespace SimuRest.Core.Services.Http;

/// <summary>
/// Processes all incoming requests.
/// </summary>
public class RequestHandler
{
    private readonly ResponseWriter _writer;
    
    private readonly Router _router;
    
    private readonly Parser _parser;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestHandler"/>.
    /// </summary>
    /// <param name="router">The <see cref="Router"/> to match routes.</param>
    /// <param name="parser">the <see cref="Parser"/> to parse <see cref="SimuRequest"/>.</param>
    /// <param name="writer">The <see cref="ResponseWriter"/> to write to the response file stream.</param>
    public RequestHandler(Router router, Parser parser, ResponseWriter writer)
    {
        _writer = writer;
        _router = router;
        _parser = parser;
    }

    /// <summary>
    /// Processes the <see cref="HttpListenerContext"/> and does all the required steps to write a response into the <see cref="HttpListenerResponse"/> as an asynchronous operation.
    /// </summary>
    /// <param name="ctx">The <see cref="HttpListenerContext"/> to process.</param>
    public async Task Handle(HttpListenerContext ctx)
    {
        HttpContextAdapter adapter = new HttpContextAdapter(ctx);
        SimuRequest? req = GetSimuRequestFromContext(adapter);
        SimuResponse response = await GetSimuResponseFromRouter(req);
        
        await WriteToStream(adapter, response);
    }
    
    /// <summary>
    /// Get a <see cref="SimuRequest"/> from a <see cref="IHttpContext"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="IHttpContext"/>.</param>
    /// <returns>The final <see cref="SimuRequest"/>.</returns>
    public SimuRequest? GetSimuRequestFromContext(IHttpContext ctx)
    {
        return _parser.Parse(ctx);
    }

    /// <summary>
    /// Get a <see cref="SimuResponse"/> from a <see cref="SimuRequest"/> as an asynchronous operation.
    /// </summary>
    /// <param name="req">The <see cref="SimuRequest"/>.</param>
    /// <returns>The final <see cref="SimuResponse"/>.</returns>
    public async Task<SimuResponse> GetSimuResponseFromRouter(SimuRequest? req)
    {
        return await _router.Handle(req);
    }

    /// <summary>
    /// Write to the file stream of the <see cref="HttpListenerResponse"/>.
    /// </summary>
    /// <param name="ctx">The <see cref="IHttpContext"/> the stream writes to.</param>
    /// <param name="response">The <see cref="SimuResponse"/> to write into file stream.</param>
    public async Task WriteToStream(IHttpContext ctx, SimuResponse response)
    {
        await _writer.Write(ctx, response);
    }
}