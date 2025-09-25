using SimuRest.Core.Models;
namespace SimuRest.Tests.Models;

public class RouteTests
{
    public static IEnumerable<object[]> InvalidRequests =>
        new List<object[]>
            {
                new object[] { null, null },
                new object[] { null, "" },
                new object[] { null, "   " },
                new object[] { HttpMethod.Get, null },
                new object[] { HttpMethod.Get, "" },
                new object[] { HttpMethod.Get, "   " },
                new object[] { HttpMethod.Post, null },
                new object[] { HttpMethod.Post, "" },
                new object[] { HttpMethod.Post, "   " },
            };
    
    [Theory]
    [MemberData(nameof(InvalidRequests))]
    public void Init_ArgumentsAreEmptyOrNull_ThrowsArgumentException(HttpMethod method, string path)
    {
        Assert.Throws<ArgumentException>(() => new Route(method, path));
    }
}