using SimuRest.Core.Models;
using SimuRest.Core.Services.Http;
using SimuRest.Core.Services.Perseverance;

namespace SimuRest.Core.Services.Builders;

/// <summary>
/// Provides access to a builder for setting up a <see cref="SimuServer"/>.
/// </summary>
public class SimuServerBuilder
{
    /// <summary>
    /// Gets or Sets the <see cref="SimuServer"/>.
    /// </summary>
    public SimuServer Server { get; }

    public ServerMemory Memory => Server.Memory;
    
    /// <summary>
    /// Initializes a new instance of a <see cref="SimuServerBuilder"/> with a given <see cref="SimuServer"/>.
    /// </summary>
    /// <param name="server">The <see cref="SimuServer"/> to setup and build.</param>
    public SimuServerBuilder(SimuServer server)
    {
        Server = server;
    }
    
    /// <summary>
    /// Initializes a new instance of a <see cref="SimuServerBuilder"/>. Initializes a new <see cref="SimuServer"/> on its own.
    /// </summary>
    public SimuServerBuilder()
    {
        Server = new SimuServer(new ServerMemory(), new RouteTable());
    }

    /// <summary>
    /// Sets up a <see cref="RouteRule"/>.
    /// </summary>
    /// <param name="method">The <see cref="HttpMethod"/> of the specified response.</param>
    /// <param name="path">The path of the specified response.</param>
    /// <returns>A <see cref="RouteRuleBuilder"/> to setup the <see cref="RouteRule"/>.</returns>
    public RouteRuleBuilder Setup(HttpMethod method, string path)
    {   
        RouteRuleBuilder routeRuleBuilder = new RouteRuleBuilder(this, new Route(method, path));
        return routeRuleBuilder;
    }

    /// <summary>
    /// Sets the <see cref="Port"/> of the <see cref="SimuServer"/>.
    /// </summary>
    /// <param name="port"></param>
    /// <returns></returns>
    public SimuServerBuilder Port(int port)
    {
        Server.Port = port;
        return this;
    }
}