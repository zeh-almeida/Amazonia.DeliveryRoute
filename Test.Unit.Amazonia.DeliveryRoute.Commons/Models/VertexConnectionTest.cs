using Amazonia.DeliveryRoute.Commons.Models;

namespace Test.Unit.Amazonia.DeliveryRoute.Commons.Models;

public sealed record VertexConnectionTest
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

    private static Vertex<Position> ValidVertex { get; } = new Vertex<Position>
    {
        Value = ValidPosition,
    };
    #endregion

    #region Equality
    [Fact]
    public void Equals_NullGridItem_IsFalse()
    {
        var itemDistance = new VertexConnection<Position>
        {
            Other = ValidVertex,
            Value = ValidDistance,
        };

        Assert.False(itemDistance.Equals(null));
    }

    [Fact]
    public void Equals_UnknownType_IsFalse()
    {
        var itemDistance = new VertexConnection<Position>
        {
            Other = ValidVertex,
            Value = ValidDistance,
        };

        Assert.False(object.Equals(itemDistance, 0));
    }

    [Fact]
    public void Equals_NullUnknownType_IsFalse()
    {
        var itemDistanceA = new VertexConnection<Position>
        {
            Other = ValidVertex,
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
            Other = ValidVertex,
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
            Other = ValidVertex,
            Value = ValidDistance,
        };

        var itemDistanceB = new VertexConnection<Position>
        {
            Other = ValidVertex,
            Value = ValidDistance + 1,
        };

        Assert.True(object.Equals(itemDistanceA, itemDistanceB));
    }

    [Fact]
    public void HashCode_Calculates()
    {
        var itemDistance = new VertexConnection<Position>
        {
            Other = ValidVertex,
            Value = ValidDistance,
        };

        var expected = HashCode.Combine(ValidVertex);
        Assert.Equal(expected, itemDistance.GetHashCode());
    }
    #endregion

    [Fact]
    public void ToString_IsCorrect()
    {
        const string expected = "C(P(A1) | 1.0)";

        var itemDistance = new VertexConnection<Position>
        {
            Other = ValidVertex,
            Value = ValidDistance,
        };

        Assert.Equal(expected, itemDistance.ToString());
    }

    #region Comparisons
    [Theory]
    [InlineData("A", "A", 0, 0, 0)]
    [InlineData("A", "A", 0, 1, -1)]
    [InlineData("A", "A", 1, 0, 1)]
    [InlineData("B", "A", 0, 0, 1)]
    [InlineData("A", "B", 0, 0, -1)]
    public void CompareTo_Executes(
        string itemA,
        string itemB,
        int distanceA,
        int distanceB,
        int expected)
    {
        var vertexA = new Vertex<string>
        {
            Value = itemA,
        };

        var vertexB = new Vertex<string>
        {
            Value = itemB,
        };

        var connectionA = new VertexConnection<string>
        {
            Other = vertexA,
            Value = distanceA,
        };

        var connectionB = new VertexConnection<string>
        {
            Other = vertexB,
            Value = distanceB,
        };

        Assert.Equal(expected, connectionA.CompareTo(connectionB));
    }

    [Fact]
    public void CompareTo_Null_AlwaysPositive()
    {
        var vertex = new Vertex<string>
        {
            Value = ValidX,
        };

        var connection = new VertexConnection<string>
        {
            Other = vertex,
            Value = ValidDistance,
        };

        Assert.True(connection.CompareTo(null) > 0);
    }
    #endregion

    #region RelatedTo
    [Fact]
    public void RelatedTo_Item_IsTrue()
    {
        var itemDistance = new VertexConnection<Position>
        {
            Other = ValidVertex,
            Value = ValidDistance,
        };

        Assert.Equal(ValidVertex, itemDistance.Other);
    }

    [Fact]
    public void RelatedTo_UnknownItem_IsFalse()
    {
        var itemDistance = new VertexConnection<Position>
        {
            Other = ValidVertex,
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
