using SimuRest.Core.Models;
using SimuRest.Core.Services;

namespace SimuRest.Tests.Services.Http;

public class RouteTableTests
{
    [Fact]
    public void Insert_RouteDoesExist_DoesNotAddOrOverride()
    {
        RouteTable table = new RouteTable();
        RouteRule rule = new RouteRule(new Route(HttpMethod.Get, "/foo"), 
            req => new SimuResponse(200, ""));
        var key = (rule.Route.Method, rule.Route.Path);
        table.Routes[key] = rule;

        Assert.Single(table.Routes);
        
        RouteRule rule2 = new RouteRule(new Route(HttpMethod.Get, "/foo"), 
            req => new SimuResponse(200, ""));
        table.Insert(rule2);
        
        Assert.Single(table.Routes);
    }

    [Fact]
    public void Insert_RouteRuleDoesNotExist_AddsSuccessfully()
    {
        RouteTable table = new RouteTable();
        RouteRule rule = new RouteRule(new Route(HttpMethod.Get, "/foo"), 
            req => new SimuResponse(200, ""));
        table.Insert(rule);

        Assert.Single(table.Routes);
    }

    [Fact]
    public void Match_RouteRuleExists_ReturnsRouteRule()
    {
        RouteTable table = new RouteTable();
        SimuRequest req = new SimuRequest(HttpMethod.Get, "/foo");
        RouteRule rule = new RouteRule(new Route(HttpMethod.Get, "/foo"), 
            req => new SimuResponse(200, ""));
        var key = (rule.Route.Method, rule.Route.Path);
        table.Routes[key] = rule;

        Assert.Equal(rule, table.Match(req));
    }

    [Fact]
    public void Match_RouteRuleDoesNotExist_ThrowsKeyNotFoundException()
    {
        RouteTable table = new RouteTable();
        SimuRequest req = new SimuRequest(HttpMethod.Get, "/foo");


        Assert.Throws<KeyNotFoundException>(() => table.Match(req));
    }
}