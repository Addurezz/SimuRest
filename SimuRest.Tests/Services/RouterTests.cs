using SimuRest.Core.Models;
using SimuRest.Core.Services;
using SimuRest.Core.Services.Http;

namespace SimuRest.Tests.Services;

public class RouterTests
{
    [Fact]
    public void Handle_RequestIsNull_ReturnsStatus400()
    {
        var router = new Router();
        SimuRequest? req = null;

        var desired = new SimuResponse(400, "Bad request");
        Assert.Equal(desired, router.Handle(req));
    }

    [Fact]
    public void Handle_RequestIsValid_ReturnsStatus200()
    {
        var request = new SimuRequest(HttpMethod.Get, "/foo");
        var response = new SimuResponse(200, "ok");
        var routeRule = new RouteRule(new Route(HttpMethod.Get, "/foo"), req => new SimuResponse(200, "ok"));
        var handler = new Router();
        handler.Table.Insert(routeRule);
        
        Assert.Equal(response, handler.Handle(request));
    }

    [Fact]
    public void Handle_RequestIsValid_RouteNotFound_ReturnsStatus404()
    {
        var request = new SimuRequest(HttpMethod.Get, "/foo");
        var response = new SimuResponse(404, "");
        var routeRule = new RouteRule(new Route(HttpMethod.Get, "/foo"), req => new SimuResponse(404, ""));
        var handler = new Router();
        handler.Table.Insert(routeRule);
        
        Assert.Equal(response, handler.Handle(request));
    }
}