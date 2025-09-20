using System.Reflection.Metadata;
using SimuRest.Core.Models;

namespace SimuRest.Core.Services;

public class RouteHandler
{
    public RouteTable Table { get; set; } = new();

    public SimuResponse Handle(SimuRequest request)
    {
        if (request is null) throw new ArgumentException("Request cannot be null", nameof(request));
        
        try
        {
            RouteRule rule = Table.Match(request);

            return rule.Handler(request);
        }

        catch (KeyNotFoundException e)
        {
            return new SimuResponse(404, "Route not found");
        }
    }
}