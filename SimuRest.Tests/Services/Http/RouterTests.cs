using SimuRest.Core.Models;
using SimuRest.Core.Services.Http;

namespace SimuRest.Tests.Services.Http;

public class RouterTests
{
    [Fact]
    public async Task Handle_RequestIsNull_ReturnsStatus400()
    {
        var router = new Router();
        SimuRequest? req = null;

        Assert.Equal(SimuResponse.BadRequest, await router.Handle(req));
    }

    [Fact]
    public async Task Handle_RequestIsValid_ReturnsStatus200()
    {
        var request = new SimuRequest(HttpMethod.Get, "/foo");
        var response = new SimuResponse(200, "ok");
        var routeRule = new RouteRule(new Route(HttpMethod.Get, "/foo"), req => new SimuResponse(200, "ok"));
        var handler = new Router();
        handler.Table.Insert(routeRule);
        
        Assert.Equal(response, await handler.Handle(request));
    }

    [Fact]
    public async Task Handle_RequestIsValid_RouteNotFound_ReturnsStatus404()
    {
        var request = new SimuRequest(HttpMethod.Get, "/foo");
        var routeRule = new RouteRule(new Route(HttpMethod.Get, "/"), req => new SimuResponse(111, ""));
        var handler = new Router();
        handler.Table.Insert(routeRule);
        
        Assert.Equal(SimuResponse.NotFound, await handler.Handle(request));
    }
}