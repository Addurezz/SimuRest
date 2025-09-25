namespace SimuRest.Core.Models;

/// <summary>
/// Determines what the response of the server is on a given <see cref="Route"/>
/// </summary>
public class RouteRule
{
    /// <summary>
    /// Gets or Sets the <see cref="Route"/> of the <see cref="RouteRule"/>
    /// </summary>
    public Route Route { get; set; }
    
    /// <summary>
    /// Gets or Sets the response function of the <see cref="RouteRule"/>
    /// </summary>
    public Func<SimuRequest, SimuResponse>? Handler { get; set; }
    
    /// <summary>
    /// Gets or Sets the delay (in milliseconds) applied when executing the <see cref="Handler"/> via <see cref="Execute"/>
    /// </summary>
    public int Delay { get; set; }

    /// <summary>
    /// Initializes a new instance of a <see cref="RouteRule"/>
    /// </summary>
    /// <param name="route">Sets the <see cref="Route"/></param>
    /// <param name="handler">Sets the response function (<see cref="Handler"/>)</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="route"/> is null</exception>
    public RouteRule(Route route, Func<SimuRequest, SimuResponse>? handler)
    {
        if (route is null) throw new ArgumentNullException();
        
        Route = route;
        Handler = handler;
    }
    
    /// <summary>
    /// Executes the <see cref="Handler"/> using the provided <see cref="SimuRequest"/>
    /// </summary>
    /// <param name="request">The <see cref="SimuRequest"/> to process.</param>
    /// <returns>A <see cref="SimuResponse"/> produced by the <see cref="Handler"/>.
    /// </returns>
    public async Task<SimuResponse> Execute(SimuRequest request)
    {
        if (Handler is null) return new SimuResponse(200, "");
        await Task.Delay(Delay);
        return Handler(request);
    }
}