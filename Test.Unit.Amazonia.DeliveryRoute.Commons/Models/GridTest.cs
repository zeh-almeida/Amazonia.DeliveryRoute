using Amazonia.DeliveryRoute.Commons.Models;

namespace Test.Unit.Amazonia.DeliveryRoute.Commons.Models;

public sealed record GridTest
{
    #region Constants
    private const string ValidX = "A";

    private const int ValidY = 1;

    private static Position ValidPosition { get; } = new Position
    {
        X = ValidX,
        Y = ValidY,
    };
    #endregion

    #region Adding Items
    [Fact]
    public void AddItem_New_Succeeds()
    {
        var itemA = new Vertex<Position>
        {
            Value = ValidPosition,
        };

        var subject = new Grid<Position>();

        var result = subject.AddItem(itemA);
        Assert.True(result);
    }

    [Fact]
    public void AddItem_Duplicate_Fails()
    {
        var itemA = new Vertex<Position>
        {
            Value = ValidPosition,
        };

        var subject = new Grid<Position>();

        var firstAttempt = subject.AddItem(itemA);
        Assert.True(firstAttempt);

        var secondAttempt = subject.AddItem(itemA);
        Assert.False(secondAttempt);
    }
    #endregion

    #region Removing Items
    [Fact]
    public void RemoveItem_Existing_Succeeds()
    {
        var itemA = new Vertex<Position>
        {
            Value = ValidPosition,
        };

        var subject = new Grid<Position>();

        var added = subject.AddItem(itemA);
        Assert.True(added);

        var removed = subject.RemoveItem(itemA);
        Assert.True(removed);
    }

    [Fact]
    public void RemoveItem_Unknown_Fails()
    {
        var itemA = new Vertex<Position>
        {
            Value = ValidPosition,
        };

        var subject = new Grid<Position>();

        var removed = subject.RemoveItem(itemA);
        Assert.False(removed);
    }
    #endregion

    #region Find Items
    [Fact]
    public void FindItem_Existing_Succeeds()
    {
        var itemA = new Vertex<Position>
        {
            Value = ValidPosition,
        };

        var subject = new Grid<Position>();

        var added = subject.AddItem(itemA);
        Assert.True(added);

        var found = subject.FindItem(ValidPosition);
        Assert.NotNull(found);
        Assert.Equal(itemA, found);
    }

    [Fact]
    public void FindItem_Unknown_Fails()
    {
        var subject = new Grid<Position>();

        var found = subject.FindItem(ValidPosition);
        Assert.Null(found);
    }
    #endregion

    #region Enumeration
    [Fact]
    public void AsEnumerable_ReturnsItems()
    {
        var itemA = new Vertex<Position>
        {
            Value = ValidPosition,
        };

        var subject = new Grid<Position>();

        var added = subject.AddItem(itemA);
        Assert.True(added);

        var items = subject.AsEnumerable();
        Assert.NotNull(items);
        Assert.Contains(itemA, items);
    }

    [Fact]
    public void Count_NoItems_ReturnsZero()
    {
        var itemA = new Vertex<Position>
        {
            Value = ValidPosition,
        };

        var subject = new Grid<Position>();
        Assert.Empty(subject.AsEnumerable());

        var added = subject.AddItem(itemA);

        Assert.True(added);
        _ = Assert.Single(subject.AsEnumerable());
    }

    [Fact]
    public void Count_WithItems_ReturnsCount()
    {
        var itemA = new Vertex<Position>
        {
            Value = ValidPosition,
        };

        var subject = new Grid<Position>();
        var added = subject.AddItem(itemA);

        Assert.True(added);
        _ = Assert.Single(subject.AsEnumerable());
    }
    #endregion

    [Fact]
    public void ToString_IsCorrect()
    {
        const string expected = "G([V(P(A1)), V(P(A2))])";

        var itemA = new Vertex<Position>
        {
            Value = ValidPosition,
        };

        var itemB = new Vertex<Position>
        {
            Value = new Position
            {
                X = ValidX,
                Y = ValidY + 1,
            },
        };

        var subject = new Grid<Position>();

        var addedA = subject.AddItem(itemA);
        Assert.True(addedA);

        var addedB = subject.AddItem(itemB);
        Assert.True(addedB);

        Assert.Equal(expected, subject.ToString());
    }
}
