namespace SimuRest.Core.Models;

public class RouteRule
{
    public Route Route { get; set; }
    public Func<SimuRequest, SimuResponse>? Handler { get; set; }

    public RouteRule(Route route, Func<SimuRequest, SimuResponse>? handler)
    {
        if (route is null) throw new ArgumentException("Route cannot be null", nameof(route));
        
        Route = route;
        Handler = handler;
        
    }
    
    public SimuResponse Execute(SimuRequest req)
    {
        if (Handler is null) return new SimuResponse(404, "Missing response");
        return Handler(req);
    }
}