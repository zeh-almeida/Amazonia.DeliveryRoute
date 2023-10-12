using Amazonia.DeliveryRoute.Commons.Extensions;

namespace Test.Amazonia.DeliveryRoute.Commons.Extensions;

public sealed record StringExtensionsTest
{
    [Theory]
    [InlineData("A", 0)]
    [InlineData("Z", 25)]
    [InlineData("AA", 26)]
    [InlineData("XFD", 16383)]
    public void AsColumnIndex_Succeeds(string value, int expected)
    {
        var result = value.AsColumnIndex();
        Assert.Equal(expected, result);
    }
}
