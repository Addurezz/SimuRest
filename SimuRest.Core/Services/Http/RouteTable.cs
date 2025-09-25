using SimuRest.Core.Models;
namespace SimuRest.Core.Services.Http;

/// <summary>
/// Provides In-Memory storage for paths and their <see cref="RouteRule"/>.
/// </summary>
public class RouteTable
{
    /// <summary>
    /// Stores the <see cref="RouteRule"/> with (<see cref="HttpMethod"/>, <see cref="String"/>) as key.
    /// </summary>
    public Dictionary<(HttpMethod, string), RouteRule> Routes { get; set; } = new();

    
    /// <summary>
    /// Matches the specified <see cref="SimuRequest"/> to existing keys in <see cref="Routes"/>.
    /// </summary>
    /// <param name="request">The <paramref name="request"/> to get the <see cref="Route"/> from. </param>
    /// <returns>A <see cref="RouteRule"/> of the matching <see cref="Route"/>. </returns>
    /// <exception cref="KeyNotFoundException">Throws if <see cref="Routes"/> does not contain specified key.</exception>
    public RouteRule Match(SimuRequest request)
    {
        (HttpMethod, string) key = (request.Method, request.Path);

        RouteRule? rule;

        if (!Routes.TryGetValue(key, out rule)) throw new KeyNotFoundException();

        return rule;
    }
    
    /// <summary>
    /// Inserts a <see cref="RouteRule"/> into <see cref="Routes"/>.
    /// </summary>
    /// <param name="rule">The <see cref="RouteRule"/> to insert.</param>
    /// <exception cref="ArgumentNullException">Throws if <paramref name="rule"/> is null.</exception>
    public void Insert(RouteRule? rule)
    {
        if (rule is null) throw new ArgumentNullException();

        (HttpMethod, string) key = (rule.Route.Method, rule.Route.Path);
        Console.WriteLine($"Inserting route: {rule.Route.Method} {rule.Route.Path}");
        
        if (!Routes.TryAdd(key, rule)) ;
    }
}