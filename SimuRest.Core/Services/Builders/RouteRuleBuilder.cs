using SimuRest.Core.Models;
using SimuRest.Core.Services.Http;
using SimuRest.Core.Services.Perseverance;

namespace SimuRest.Core.Services.Builders;

/// <summary>
/// Provides access to a builder for setting up a <see cref="RouteRule"/>.
/// </summary>
public class RouteRuleBuilder
{
    private readonly SimuServerBuilder _serverBuilder;
    private readonly ServiceProvider _provider;
    private readonly RouteRule _rule;

    /// <summary>
    /// Initializes a new instance of a <see cref="RouteRuleBuilder"/>.
    /// </summary>
    /// <param name="serverBuilder">The <see cref="SimuServerBuilder"/> that requested the <see cref="RouteRuleBuilder"/>.</param>
    /// <param name="route">The <see cref="Route"/> to build the <see cref="RouteRule"/> from.</param>
    /// <param name="provider">The <see cref="ServiceProvider"/> that stores the services needed.</param>
    public RouteRuleBuilder(SimuServerBuilder serverBuilder, Route route, ServiceProvider provider)
    {
        _provider = provider;
        _serverBuilder = serverBuilder;
        _rule = new RouteRule(route, null);
    }

    /// <summary>
    /// Sets up the response of the server for the <see cref="RouteRule"/>.
    /// Setup can be finished with <see cref="Apply"/>.
    /// </summary>
    /// <param name="handler">The response handler of the server.</param>
    /// <returns>The <see cref="RouteRuleBuilder"/>.</returns>
    /// <exception cref="ApplicationException">Throws if <paramref name="handler"/> is null.</exception>
    public RouteRuleBuilder Responds(Func<SimuRequest, SimuResponse> handler)
    {
        if (_rule is null) throw new ArgumentNullException();
        _rule.Handler = handler;
        return this;
    }

    /// <summary>
    /// Sets up an artificial <see cref="Delay"/> of the <see cref="RouteRule"/>.
    /// Setup can be finished with <see cref="Apply"/>.
    /// </summary>
    /// <param name="ms">The time to delay (in milliseconds).</param>
    /// <returns>The <see cref="RouteRuleBuilder"/>.</returns>
    public RouteRuleBuilder Delay(int ms)
    {
        if (ms > 0)
            _rule!.Delay = ms;

        return this;
    }

    /// <summary>
    /// Configures the current <see cref="RouteRule"/> to store an incoming POST request body in memory.
    /// </summary>
    /// <returns>The <see cref="RouteRuleBuilder"/>.</returns>
    /// <exception cref="InvalidOperationException">Throws if the <see cref="RouteRule"/> to configure is null or is not a POST request.</exception>
    public RouteRuleBuilder SaveInMemory()
    {
        if (_rule is null || _rule.Route.Method != HttpMethod.Post) throw new InvalidOperationException();

        ServerMemory memory = _provider.GetService<ServerMemory>();
        
        _rule.Handler = req =>
        {
            try
            {
                memory.Save(req.Path, req.Body);
                return new SimuResponse(200, "Saved");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new SimuResponse(401, "Not saved");
            }
        };

        return this;
    }

    /// <summary>
    /// Configures the current <see cref="RouteRule"/> to respond to an incoming GET request from an in memory source.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public RouteRuleBuilder RespondFromMemory()
    {
        if (_rule is null || _rule.Route.Method != HttpMethod.Get) throw new InvalidOperationException();

        ServerMemory memory = _provider.GetService<ServerMemory>();

        _rule.Handler = req =>
        {
            try
            {
                var data = memory.Get(req.Path);
                if (data is string s)
                    return new SimuResponse(200, s);

                return SimuResponse.NotFound;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new SimuResponse(402, "Something went wrong regarding the server response.");
            }
        };
 
        return this;
    }
    
    /// <summary>
    /// Finalizes the <see cref="RouteRule"/> setup and saves it to the <see cref="RouteTable"/> of the <see cref="SimuServer"/>.
    /// </summary>
    /// <returns>The <see cref="SimuServerBuilder"/>.</returns>
    public SimuServerBuilder Apply()
    {
        Router router = _provider.GetService<Router>();
        router.InsertRouteRule(_rule);
        return _serverBuilder;
    }
}