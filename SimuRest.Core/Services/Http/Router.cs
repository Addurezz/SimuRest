using SimuRest.Core.Models;

namespace SimuRest.Core.Services.Http;

/// <summary>
/// Represents a Router for the <see cref="SimuServer"/>. Provides access to route matching with <see cref="RouteTable"/>.
/// </summary>
public class Router
{   
    /// <summary>
    /// Gets or Sets the <see cref="RouteTable"/>.
    /// </summary>
    public RouteTable Table { get; set; } = new();
    
    /// <summary>
    /// Process specified <see cref="SimuRequest"/> as asynchronous operation.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <returns>The final <see cref="SimuResponse"/> of the server.</returns>
    public async Task<SimuResponse> Handle(SimuRequest? request)
    {
        if (request is null) return SimuResponse.BadRequest;
        
        try
        {
            RouteRule rule = Table.Match(request);

            return await rule.Execute(request);
        }

        catch (KeyNotFoundException e)
        {
            return SimuResponse.NotFound;
        }
    }
}