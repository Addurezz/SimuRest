using SimuRest.Core.Models;
using SimuRest.Core.Services.Http;

namespace SimuRest.Core.Services.Builders;

public class SimuServerBuilder
{
    public SimuServer Server { get; set; }
    public SimuServerBuilder(SimuServer server)
    {
        Server = server;
    }
    
    public SimuServerBuilder()
    {
        Router r = new Router();
        ResponseWriter w = new ResponseWriter();
        Parser p = new Parser();
        Server = new SimuServer(r, w, p);
    }

    public RouteRuleBuilder Setup(HttpMethod method, string Path)
    {   
        RouteRuleBuilder routeRuleBuilder = new RouteRuleBuilder(this, new Route(method, Path));
        return routeRuleBuilder;
    }

    public SimuServerBuilder Port(int port)
    {
        Server.Port = port;
        return this;
    }
}