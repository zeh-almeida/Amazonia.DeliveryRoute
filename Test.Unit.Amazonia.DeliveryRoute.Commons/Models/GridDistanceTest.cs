using Amazonia.DeliveryRoute.Commons.Models;
using Newtonsoft.Json.Linq;

namespace Test.Unit.Amazonia.DeliveryRoute.Commons.Models;

public sealed record GridDistanceTest
{
    #region Constants
    private const string ValidX = "A";

    private const int ValidY = 1;

    private const decimal ValidDistance = 1;

    private static Position ValidPositionA { get; } = new Position
    {
        X = ValidX,
        Y = ValidY,
    };

    private static Position ValidPositionB { get; } = new Position
    {
        X = ValidX,
        Y = ValidY + 1,
    };

    private static GridItem<string> ItemB { get; } = new GridItem<string>
    {
        Position = ValidPositionB,
    };
    #endregion

    #region Equality
    [Fact]
    public void Equals_NullGridItem_IsFalse()
    {
        var itemDistance = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        Assert.False(itemDistance.Equals(null));
    }

    [Fact]
    public void Equals_UnknownType_IsFalse()
    {
        var itemDistance = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        Assert.False(object.Equals(itemDistance, 0));
    }

    [Fact]
    public void Equals_NullUnknownType_IsFalse()
    {
        var itemDistanceA = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        object? itemDistanceB = null;
        Assert.False(object.Equals(itemDistanceA, itemDistanceB));
    }

    [Fact]
    public void Equals_DifferentItem_IsFalse()
    {
        var differentGridItem = new GridItem<string>
        {
            Position = new Position
            {
                X = ValidX,
                Y = ValidY + 2,
            },
        };

        var itemDistanceA = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        var itemDistanceB = new GridDistance<string>
        {
            Other = differentGridItem,
            Value = ValidDistance,
        };

        Assert.False(object.Equals(itemDistanceA, itemDistanceB));
    }

    [Fact]
    public void Equals_SamePosition_IsTrue()
    {
        var itemDistanceA = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        var itemDistanceB = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance + 1,
        };

        Assert.True(object.Equals(itemDistanceA, itemDistanceB));
    }

    [Fact]
    public void HashCode_Calculates()
    {
        var itemDistance = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        var expected = HashCode.Combine(ItemB);
        Assert.Equal(expected, itemDistance.GetHashCode());
    }
    #endregion

    [Fact]
    public void ToString_IsCorrect()
    {
        const string expected = "(A2 | 1.0)";

        var itemDistance = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        Assert.Equal(expected, itemDistance.ToString());
    }

    #region RelatedTo

    [Fact]
    public void RelatedTo_Item_IsTrue()
    {
        var itemDistance = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        Assert.True(itemDistance.RelatedTo(ItemB));
    }

    [Fact]
    public void RelatedTo_UnknownItem_IsFalse()
    {
        var itemDistance = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        var itemC = new GridItem<string>
        {
            Position = new Position
            {
                X = ValidX,
                Y = ValidY + 2,
            },
        };

        Assert.False(itemDistance.RelatedTo(itemC));
    }
    #endregion

    #region CoversPosition
    [Fact]
    public void CoversPosition_Item_IsTrue()
    {
        var itemDistance = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        var result = itemDistance.CoversPosition(ValidPositionB);

        Assert.NotNull(result);
        Assert.Equal(ItemB, result);
    }

    [Fact]
    public void CoversPosition_UnknownItem_IsNull()
    {
        var itemDistance = new GridDistance<string>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        var positionC = new Position
        {
            X = ValidX,
            Y = ValidY + 2,
        };

        Assert.Null(itemDistance.CoversPosition(positionC));
    }
    #endregion
}
