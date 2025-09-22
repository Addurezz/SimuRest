using SimuRest.Core.Models;
namespace SimuRest.Core.Services;

public class RouteTable
{
    public Dictionary<(HttpMethod, string), RouteRule> Routes { get; set; } = new();

    public RouteRule Match(SimuRequest req)
    {
        (HttpMethod, string) key = (req.Method, req.Path);

        RouteRule? rule;

        if (!Routes.TryGetValue(key, out rule)) throw new KeyNotFoundException();

        return rule;
    }

    public void Insert(RouteRule? rule)
    {
        if (rule is null) throw new ArgumentNullException();

        (HttpMethod, string) key = (rule.Route.Method, rule.Route.Path);
        Console.WriteLine($"Inserting route: {rule.Route.Method} {rule.Route.Path}");
        
        if (!Routes.TryAdd(key, rule)) ;
    }
}