namespace SimuRest.Core.Models;

public class RouteRule
{
    public Route Route { get; }
    public Func<SimuRequest, SimuResponse> Handler { get; }

    public RouteRule(Route route, Func<SimuRequest, SimuResponse> handler)
    {
        if (route is null) throw new ArgumentException("Route cannot be null", nameof(route));
        if (handler is null) throw new ArgumentException("Handler function cannot be null", nameof(route));
        
        Route = route;
        Handler = handler;
    }
}