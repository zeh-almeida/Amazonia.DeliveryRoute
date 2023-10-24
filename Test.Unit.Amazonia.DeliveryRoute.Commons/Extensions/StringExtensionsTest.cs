using Amazonia.DeliveryRoute.Commons.Extensions;

namespace Test.Unit.Amazonia.DeliveryRoute.Commons.Extensions;

public sealed record StringExtensionsTest
{
    #region AsColumnIndex
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

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void AsColumnIndex_EmptyOrWhiteSpace_ThrowsArgumentException(string value)
    {
        _ = Assert.Throws<ArgumentException>(() => value.AsColumnIndex());
    }

    [Fact]
    public void AsColumnIndex_Null_ThrowsArgumentNullException()
    {
#pragma warning disable CS8604, CS8625 // Possible null reference argument.
        const string? value = null;
        _ = Assert.Throws<ArgumentNullException>(() => value.AsColumnIndex());
#pragma warning restore CS8604, CS8625 // Possible null reference argument.
    }

    [Theory]
    [InlineData("0")]
    [InlineData("a0")]
    [InlineData("A0")]
    [InlineData("-")]
    public void AsColumnIndex_InvalidString_ThrowsArgumentException(string value)
    {
        _ = Assert.Throws<ArgumentException>(() => value.AsColumnIndex());
    }
    #endregion

    #region AsCoordinates
    [Theory]
    [MemberData(nameof(AsCoordinatesData))]
    public void AsCoordinates_Succeeds(string value, Tuple<string, int> expected)
    {
        var result = value.AsCoordinates();

        Assert.Equal(expected.Item1, result.Item1);
        Assert.Equal(expected.Item2, result.Item2);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("A")]
    public void AsCoordinates_Fails(string value)
    {
        _ = Assert.Throws<ArgumentException>(() => value.AsCoordinates());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void AsCoordinates_InvalidString_ThrowsArgumentException(string? value)
    {
        _ = Assert.Throws<ArgumentException>(() => value?.AsCoordinates());
    }
    #endregion

    public static IEnumerable<object[]> AsCoordinatesData()
    {
        return new object[][]
        {
            ["A1",  new Tuple<string, int>("A",1)],
            ["Z9", new Tuple<string, int>("Z",9)],
            ["AA22", new Tuple<string, int>("AA",22)],
            ["XFD911", new Tuple<string, int>("XFD",911)],
        };
    }
}
