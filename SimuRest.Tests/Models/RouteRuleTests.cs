using SimuRest.Core.Models;

namespace SimuRest.Tests.Models;

public class RouteRuleTests
{
    public static IEnumerable<object[]> InvalidInitArguments =>
        new List<object[]>
        {
            new object[] { null, new Func<SimuRequest, SimuResponse>(req => new SimuResponse(1, "a")) },
        };
    
    [Theory]
    [MemberData(nameof(InvalidInitArguments))]
    public void Init_RouteOrHandlerIsNull_ThrowsArgumentException(Route route, Func<SimuRequest, SimuResponse> func)
    {
        Assert.Throws<ArgumentException>(() => new RouteRule(route, func));
    }
}