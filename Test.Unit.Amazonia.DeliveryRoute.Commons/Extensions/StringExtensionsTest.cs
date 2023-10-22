﻿using Amazonia.DeliveryRoute.Commons.Extensions;

namespace Test.Unit.Amazonia.DeliveryRoute.Commons.Extensions;

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
#pragma warning disable CS8604 // Possible null reference argument.
        const string? value = null;
        _ = Assert.Throws<ArgumentNullException>(() => value.AsColumnIndex());
#pragma warning restore CS8604 // Possible null reference argument.
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
}
