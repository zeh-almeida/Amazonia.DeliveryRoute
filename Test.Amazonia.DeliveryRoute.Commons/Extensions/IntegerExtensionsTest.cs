using Amazonia.DeliveryRoute.Commons.Extensions;

namespace Test.Amazonia.DeliveryRoute.Commons.Extensions;

public sealed record IntegerExtensionsTest
{
    [Theory]
    [InlineData(1, "A")]
    [InlineData(27, "AA")]
    [InlineData(16384, "XFD")]
    public void AsColumnName_Succeeds(int value, string expected)
    {
        var result = value.AsColumnName();
        Assert.Equal(expected, result);
    }
}
