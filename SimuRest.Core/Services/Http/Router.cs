using SimuRest.Core.Models;

namespace SimuRest.Core.Services.Http;

public class Router
{
    public RouteTable Table { get; set; } = new();

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