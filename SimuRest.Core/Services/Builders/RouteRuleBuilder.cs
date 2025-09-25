using SimuRest.Core.Models;
using SimuRest.Core.Services.Http;

namespace SimuRest.Core.Services.Builders;

/// <summary>
/// Provides access to a builder for setting up a <see cref="RouteRule"/>.
/// </summary>
public class RouteRuleBuilder
{
    private readonly SimuServerBuilder _serverBuilder;
    private readonly RouteRule? _rule;
    private readonly RouteTable _routeTable;

    /// <summary>
    /// Initializes a new instance of a <see cref="RouteRuleBuilder"/>.
    /// </summary>
    /// <param name="serverBuilder">The <see cref="SimuServerBuilder"/> that requested the <see cref="RouteRuleBuilder"/>.</param>
    /// <param name="route">The <see cref="Route"/> to build the <see cref="RouteRule"/> from.</param>
    public RouteRuleBuilder(SimuServerBuilder serverBuilder, Route route)
    {
        _serverBuilder = serverBuilder;
        _routeTable = serverBuilder.Server.Router.Table;
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
    /// Finalizes the <see cref="RouteRule"/> setup and saves it to the <see cref="RouteTable"/> of the <see cref="SimuServer"/>.
    /// </summary>
    /// <returns>The <see cref="SimuServerBuilder"/>.</returns>
    public SimuServerBuilder Apply()
    {
        _routeTable.Insert(_rule);
        return _serverBuilder;
    }
}