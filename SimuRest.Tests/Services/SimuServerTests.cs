using System.Net;
using Moq;
using SimuRest.Core.Services;
using SimuRest.Core.Services.Http;
using Xunit;

public class SimuServerTests
{
    [Fact]
    public void Start_ShouldAddPrefix()
    {
        var router = new Mock<Router>();
        var writer = new Mock<ResponseWriter>();
        var parser = new Mock<Parser>();

        var server = new SimuServer(router.Object, writer.Object, parser.Object);

        // Listener ist public ⇒ wir können reinschauen
        Assert.Contains($"http://localhost:{server.Port}/", server.Listener.Prefixes);
    }

    [Fact]
    public void Stop_ShouldStopListener()
    {
        var router = new Mock<Router>();
        var writer = new Mock<ResponseWriter>();
        var parser = new Mock<Parser>();

        var server = new SimuServer(router.Object, writer.Object, parser.Object);

        server.Listener.Start();
        Assert.True(server.Listener.IsListening);

        server.Stop();

        Assert.False(server.Listener.IsListening);
    }
}