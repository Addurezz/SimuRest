using SimuRest.Core.Models;

namespace SimuRest.Core.Services.Builders;

public class RouteRuleBuilder
{
    private SimuServerBuilder _serverBuilder;
    private RouteRule? _rule;
    private RouteTable _routeTable;

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

    public SimuServerBuilder End()
    {
        _routeTable.Insert(_rule);
        return _serverBuilder;
    }
}