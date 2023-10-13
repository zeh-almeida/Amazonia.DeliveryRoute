using Amazonia.DeliveryRoute.Commons.Extensions;

namespace Test.Unit.Amazonia.DeliveryRoute.Commons.Extensions;

public sealed record IntegerExtensionsTest
{
    [Theory]
    [InlineData(0, "A")]
    [InlineData(25, "Z")]
    [InlineData(26, "AA")]
    [InlineData(16383, "XFD")]
    public void AsColumnName_Succeeds(int value, string expected)
    {
        var result = value.AsColumnName();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AsColumnName_NegativeValue_Fails()
    {
        const int value = -1;
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => value.AsColumnName());
    }
}
