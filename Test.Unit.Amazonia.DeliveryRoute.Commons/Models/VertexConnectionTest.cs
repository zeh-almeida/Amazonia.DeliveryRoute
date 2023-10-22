using Amazonia.DeliveryRoute.Commons.Models;

namespace Test.Unit.Amazonia.DeliveryRoute.Commons.Models;

public sealed record VertexConnectionTest
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

    private static Vertex<Position> ItemB { get; } = new Vertex<Position>
    {
        Value = ValidPositionB,
    };
    #endregion

    #region Equality
    [Fact]
    public void Equals_NullGridItem_IsFalse()
    {
        var itemDistance = new VertexConnection<Position>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        Assert.False(itemDistance.Equals(null));
    }

    [Fact]
    public void Equals_UnknownType_IsFalse()
    {
        var itemDistance = new VertexConnection<Position>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        Assert.False(object.Equals(itemDistance, 0));
    }

    [Fact]
    public void Equals_NullUnknownType_IsFalse()
    {
        var itemDistanceA = new VertexConnection<Position>
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
        var differentGridItem = new Vertex<Position>
        {
            Value = new Position
            {
                X = ValidX,
                Y = ValidY + 2,
            },
        };

        var itemDistanceA = new VertexConnection<Position>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        var itemDistanceB = new VertexConnection<Position>
        {
            Other = differentGridItem,
            Value = ValidDistance,
        };

        Assert.False(object.Equals(itemDistanceA, itemDistanceB));
    }

    [Fact]
    public void Equals_SamePosition_IsTrue()
    {
        var itemDistanceA = new VertexConnection<Position>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        var itemDistanceB = new VertexConnection<Position>
        {
            Other = ItemB,
            Value = ValidDistance + 1,
        };

        Assert.True(object.Equals(itemDistanceA, itemDistanceB));
    }

    [Fact]
    public void HashCode_Calculates()
    {
        var itemDistance = new VertexConnection<Position>
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
        const string expected = "C(P(A2) | 1.0)";

        var itemDistance = new VertexConnection<Position>
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
        var itemDistance = new VertexConnection<Position>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        Assert.Equal(ItemB, itemDistance.Other);
    }

    [Fact]
    public void RelatedTo_UnknownItem_IsFalse()
    {
        var itemDistance = new VertexConnection<Position>
        {
            Other = ItemB,
            Value = ValidDistance,
        };

        var itemC = new Vertex<Position>
        {
            Value = new Position
            {
                X = ValidX,
                Y = ValidY + 2,
            },
        };

        Assert.NotEqual(itemC, itemDistance.Other);
    }
    #endregion
}
