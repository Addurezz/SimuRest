using SimuRest.Core.Models;

namespace SimuRest.Tests.Models;

public class SimuResponseTests
{
    [Theory]
    [InlineData(1, "")]
    [InlineData(1, "  ")]
    [InlineData(1, null)]
    public void Init_ArgumentsAreInvalid_ThrowsArgumentException(int status, string body)
    {
        Assert.Throws<ArgumentException>(() => new SimuResponse(status, body));
    }
}