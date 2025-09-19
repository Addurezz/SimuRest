using SimuRest.Core.Models;
using SimuRest.Core.Services;
namespace SimuRest.Tests.Services;

public class RouteHandlerTests
{
    [Fact]
    public void Handle_RequestIsNull_ThrowsArgumentException()
    {
        var handler = new RouteHandler();
        Assert.Throws<ArgumentException>(() => handler.Handle(null));
    }

    [Fact]
    public void Handle_RequestIsValid_ReturnsResponse()
    {
        var request = new SimuRequest(HttpMethod.Get, "/foo");
        var response = new SimuResponse(200, "ok");
        var routeRule = new RouteRule(new Route(HttpMethod.Get, "/foo"), req => new SimuResponse(200, "ok"));
        var handler = new RouteHandler();
        handler.Routes.Add(routeRule);
        
        Assert.Equal(response, handler.Handle(request));
    }

    [Fact]
    public void Handle_RequestIsValid_RouteNotFound_ReturnsStatus404()
    {
        var request = new SimuRequest(HttpMethod.Get, "/foo");
        var response = new SimuResponse(404, "");
        var routeRule = new RouteRule(new Route(HttpMethod.Get, "/foo"), req => new SimuResponse(404, ""));
        var handler = new RouteHandler();
        handler.Routes.Add(routeRule);
        
        Assert.Equal(response, handler.Handle(request));
    }
}