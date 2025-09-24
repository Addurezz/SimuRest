using SimuRest.Core.Models;

namespace SimuRest.Core.Services.Builders;

public class RouteRuleBuilder
{
    private readonly SimuServerBuilder _serverBuilder;
    private readonly RouteRule? _rule;
    private readonly RouteTable _routeTable;

    public RouteRuleBuilder(SimuServerBuilder serverBuilder, Route route)
    {
        _serverBuilder = serverBuilder;
        _routeTable = serverBuilder.Server.Router.Table;
        _rule = new RouteRule(route, null);
    }

    public RouteRuleBuilder Responds(Func<SimuRequest, SimuResponse> handler)
    {
        if (_rule is null) throw new ApplicationException();
        _rule.Handler = handler;
        return this;
    }

    public RouteRuleBuilder Delay(int ms)
    {
        if (ms > 0)
            _rule!.Delay = ms;

        return this;
    }

    public SimuServerBuilder End()
    {
        _routeTable.Insert(_rule);
        return _serverBuilder;
    }
}