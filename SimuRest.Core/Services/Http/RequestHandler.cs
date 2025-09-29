using System.Net;
using SimuRest.Core.Models;
using SimuRest.Core.Services.Perseverance;

namespace SimuRest.Core.Services.Http;

/// <summary>
/// Processes all incoming requests.
/// </summary>
public class RequestHandler
{
    private readonly ResponseWriter _writer;
    
    private readonly Router _router;
    
    private readonly Parser _parser;

    private readonly ServerMemory _memory;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestHandler"/>.
    /// </summary>
    /// <param name="router">The <see cref="Router"/> to match routes.</param>
    /// <param name="memory">The <see cref="ServerMemory"/> to save and retrieve response data from.</param>
    public RequestHandler(Router router, ServerMemory memory)
    {
        _memory = memory;
        _router = router;
        
        _writer = new ResponseWriter();
        _parser = new Parser();
    }

    /// <summary>
    /// Processes the <see cref="HttpListenerContext"/> and does all the required steps to write a response into the <see cref="HttpListenerResponse"/> as an asynchronous operation.
    /// </summary>
    /// <param name="ctx">The <see cref="HttpListenerContext"/> to process.</param>
    public async Task Handle(HttpListenerContext ctx)
    {
        try
        {
            HttpContextAdapter adapter = new HttpContextAdapter(ctx);
            SimuRequest? req = GetSimuRequestFromContext(adapter);
            SimuResponse response = await GetSimuResponse(req);
            await WriteToStream(adapter, response);
        }
        catch (Exception e)
        {
            await WriteToStream(new HttpContextAdapter(ctx),
                new SimuResponse(500, $"Internal server error: {e.Message}"));
        }
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
    /// <exception cref="ArgumentNullException">Throws of <param name="req"> is null.</param></exception>
    public async Task<SimuResponse> GetSimuResponse(SimuRequest? req)
    {
        if (req is null) throw new ArgumentNullException();
        
        // check if custom response was set up
        SimuResponse response = await _router.Handle(req);
        
        // if no custom response was found, check if the memory has a response 
        if (response.Equals(SimuResponse.NotFound))
        {
            var data = _memory.Get(req.Path);
            
            if (data is string s)
                response = new SimuResponse(200, s);
        }

        return response;
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