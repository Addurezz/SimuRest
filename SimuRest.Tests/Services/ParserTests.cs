using Moq;
using SimuRest.Core.Models;
using SimuRest.Core.Services.Http;

namespace SimuRest.Tests.Services;

public class ParserTests
{
    [Fact]
    public void Parse_ContextIsNull_ThrowsArgumentNullException()
    {
        var parser = new Parser();
        Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
    }

    [Fact]
    public void Parse_UrlIsNull_GetMethod_ReturnsRequest()
    {
        var parser = new Parser();
        var mockContext = new Mock<IHttpContext>();
        mockContext.Setup(m => m.Url).Returns(new Uri("http://localhost"));
        mockContext.Setup(m => m.HttpMethod).Returns("GET");

        var desired = new SimuRequest(HttpMethod.Get, "/");
        Assert.Equal(desired, parser.Parse(mockContext.Object));
    }
    
    [Fact]
    public void Parse_UrlIsNull_PostMethod_ReturnsRequest()
    {
        var parser = new Parser();
        var mockContext = new Mock<IHttpContext>();
        mockContext.Setup(m => m.Url).Returns(new Uri("http://localhost"));
        mockContext.Setup(m => m.HttpMethod).Returns("POST");

        var desired = new SimuRequest(HttpMethod.Post, "/");
        Assert.Equal(desired, parser.Parse(mockContext.Object));
    }

    [Fact]
    public void Parse_UrlIsNotNullOrEmpty_GetMethod_ReturnsRequest()
    {
        var parser = new Parser();
        var mockContext = new Mock<IHttpContext>();
        mockContext.Setup(m => m.Url).Returns(new Uri("http://localhost/foo"));
        mockContext.Setup(m => m.HttpMethod).Returns("GET");

        var desired = new SimuRequest(HttpMethod.Get, "/foo");
        Assert.Equal(desired, parser.Parse(mockContext.Object));
    }
    
    [Fact]
    public void Parse_UrlIsNotNullOrEmpty_PostMethod_ReturnsRequest()
    {
        var parser = new Parser();
        var mockContext = new Mock<IHttpContext>();
        mockContext.Setup(m => m.Url).Returns(new Uri("http://localhost/foo"));
        mockContext.Setup(m => m.HttpMethod).Returns("POST");

        var desired = new SimuRequest(HttpMethod.Post, "/foo");
        Assert.Equal(desired, parser.Parse(mockContext.Object));
    }
}