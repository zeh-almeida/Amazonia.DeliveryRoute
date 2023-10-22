using Amazonia.DeliveryRoute.Commons.Models;

namespace Test.Unit.Amazonia.DeliveryRoute.Commons.Models;

public sealed record GridItemTest
{
    #region Constants
    private const string ValidX = "A";

    private const int ValidY = 1;

    private const decimal ValidDistance = 1;

    private static Position ValidPosition { get; } = new Position
    {
        X = ValidX,
        Y = ValidY,
    };
    #endregion

    #region Equality
    [Fact]
    public void Equals_NullGridItem_False()
    {
        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        Assert.False(itemA.Equals(null));
    }

    [Fact]
    public void Equals_UnknownType_False()
    {
        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        Assert.False(object.Equals(itemA, 0));
    }

    [Fact]
    public void Equals_NullUnknownType_False()
    {
        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        object? itemB = null;
        Assert.False(object.Equals(itemA, itemB));
    }

    [Fact]
    public void Equals_SamePosition_True()
    {
        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        var itemB = new GridItem<string>
        {
            Position = ValidPosition,
        };

        Assert.True(object.Equals(itemA, itemB));
    }

    [Fact]
    public void HashCode_Calculates()
    {
        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        var expected = HashCode.Combine(ValidPosition);
        Assert.Equal(expected, itemA.GetHashCode());
    }
    #endregion

    #region Comparisons
    [Theory]
    [InlineData("A", 1, "A", 1, 0)]
    [InlineData("A", 1, "A", 2, -1)]
    [InlineData("A", 2, "A", 1, 1)]
    [InlineData("B", 1, "A", 1, 1)]
    [InlineData("A", 1, "B", 1, -1)]
    public void CompareTo_GridItem_Executes(
        string xItemA, int yItemA,
        string xItemB, int yItemB,
        int expected)
    {
        var itemA = new GridItem<string>
        {
            Position = new Position
            {
                X = xItemA,
                Y = yItemA
            },
        };

        var itemB = new GridItem<string>
        {
            Position = new Position
            {
                X = xItemB,
                Y = yItemB
            },
        };

        Assert.Equal(expected, itemA.CompareTo(itemB));
    }

    [Fact]
    public void CompareTo_NullGridItem_Executes()
    {
        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        GridItem<string>? itemB = null;
        Assert.True(itemA.CompareTo(itemB) > 0);
    }
    #endregion

    [Fact]
    public void ToString_IsCorrect()
    {
        const string expected = "(A1: [])";

        var item = new GridItem<string>
        {
            Position = ValidPosition,
        };

        Assert.Equal(expected, item.ToString());
    }

    #region Neighbors
    [Fact]
    public void AddNeighbor_Succeeds()
    {
        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        var itemB = new GridItem<string>
        {
            Position = new Position
            {
                X = ValidX,
                Y = ValidY + 1
            },
        };

        itemA.AddNeighbor(itemB, ValidDistance);

        Assert.NotEmpty(itemA.AllNeighbors());
        Assert.True(itemA.IsNeighbor(itemB));
    }

    [Fact]
    public void AddNeighbor_Duplicate_KeepsOldDistance()
    {
        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        var itemB = new GridItem<string>
        {
            Position = new Position
            {
                X = ValidX,
                Y = ValidY + 1
            },
        };

        itemA.AddNeighbor(itemB, ValidDistance);

        Assert.NotEmpty(itemA.AllNeighbors());
        Assert.True(itemA.IsNeighbor(itemB));

        var oldDistance = itemA.AllNeighbors().First();
        var newDistance = ValidDistance + 1;

        itemA.AddNeighbor(itemB, newDistance);
        var currentDistance = itemA.AllNeighbors().First();

        Assert.Equal(oldDistance.Value, currentDistance.Value);
        Assert.NotEqual(newDistance, currentDistance.Value);
    }

    [Fact]
    public void FindNeighbor_ValidNeighbor_Returns()
    {
        var otherPosition = new Position
        {
            X = ValidX,
            Y = ValidY + 1
        };

        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        var itemB = new GridItem<string>
        {
            Position = otherPosition,
        };

        itemA.AddNeighbor(itemB, ValidDistance);

        Assert.NotNull(itemA.FindNeighbor(otherPosition));
    }

    [Fact]
    public void FindNeighbor_NoNeighbor_IsNull()
    {
        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        Assert.Null(itemA.FindNeighbor(ValidPosition));
    }

    [Fact]
    public void FindNeighbor_NoMatchingNeighbor_IsNull()
    {
        var otherPosition = new Position
        {
            X = ValidX,
            Y = ValidY + 1
        };

        var unknownPosition = new Position
        {
            X = ValidX,
            Y = ValidY + 2
        };

        var itemA = new GridItem<string>
        {
            Position = ValidPosition,
        };

        var itemB = new GridItem<string>
        {
            Position = otherPosition,
        };

        itemA.AddNeighbor(itemB, ValidDistance);

        Assert.Null(itemA.FindNeighbor(unknownPosition));
        Assert.Null(itemB.FindNeighbor(unknownPosition));
    }
    #endregion
}
