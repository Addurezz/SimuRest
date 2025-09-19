using System.Reflection.Metadata;
using SimuRest.Core.Models;

namespace SimuRest.Core.Services;

public class RouteHandler
{
    public List<RouteRule> Routes { get; set; } = new();

    public SimuResponse Handle(SimuRequest request)
    {
        if (request is null) throw new ArgumentException("Request cannot be null", nameof(request));
        
        try
        {
            foreach (RouteRule route in Routes)
            {
                if (route.Route.Method == request.Method && route.Route.Path == request.Path)
                    return route.Handler(request);
            }

            return new SimuResponse(404, "");
        }

        catch
        {
            return new SimuResponse(500, "Internal Server Error");
        }
    }
}