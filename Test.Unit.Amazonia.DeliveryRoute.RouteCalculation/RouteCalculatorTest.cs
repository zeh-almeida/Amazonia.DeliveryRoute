using Amazonia.DeliveryRoute.Commons.Models;
using Amazonia.DeliveryRoute.RouteCalculation;

namespace Test.Unit.Amazonia.DeliveryRoute.RouteCalculation;

public sealed record RouteCalculatorTest
{
    #region Constants
    private const string ValidX = "A";

    private const int ValidY = 1;
    #endregion

    #region Parameter Validation
    [Fact]
    public async Task Grid_IsEmpty_Throws()
    {
        var grid = new Grid();

        var startPosition = new Position
        {
            X = ValidX,
            Y = ValidY
        };

        var endPosition = new Position
        {
            X = ValidX,
            Y = ValidY + 1
        };

        var subject = new RouteCalculator();
        var exp = await Assert.ThrowsAsync<ArgumentException>(() => subject.CalculateAsync(grid, startPosition, endPosition));

        Assert.NotNull(exp);
        Assert.Equal("grid", exp.ParamName);
    }

    [Fact]
    public async Task Start_NotInGrid_Throws()
    {
        var grid = new Grid();

        var startPosition = new Position
        {
            X = ValidX,
            Y = ValidY
        };

        var endPosition = new Position
        {
            X = ValidX,
            Y = ValidY + 1
        };

        _ = grid.AddItem(new GridItem
        {
            Position = endPosition
        });

        var subject = new RouteCalculator();
        var exp = await Assert.ThrowsAsync<ArgumentException>(() => subject.CalculateAsync(grid, startPosition, endPosition));

        Assert.NotNull(exp);
        Assert.Equal("start", exp.ParamName);
    }

    [Fact]
    public async Task Destination_NotInGrid_Throws()
    {
        var grid = new Grid();

        var startPosition = new Position
        {
            X = ValidX,
            Y = ValidY
        };

        var endPosition = new Position
        {
            X = ValidX,
            Y = ValidY + 1
        };

        _ = grid.AddItem(new GridItem
        {
            Position = startPosition
        });

        var subject = new RouteCalculator();
        var exp = await Assert.ThrowsAsync<ArgumentException>(() => subject.CalculateAsync(grid, startPosition, endPosition));

        Assert.NotNull(exp);
        Assert.Equal("destination", exp.ParamName);
    }

    [Fact]
    public async Task Start_EqualsDestination_Throws()
    {
        var grid = new Grid();

        var startPosition = new Position
        {
            X = ValidX,
            Y = ValidY
        };

        _ = grid.AddItem(new GridItem
        {
            Position = startPosition
        });

        var subject = new RouteCalculator();
        var exp = await Assert.ThrowsAsync<ArgumentException>(() => subject.CalculateAsync(grid, startPosition, startPosition));

        Assert.NotNull(exp);
        Assert.Equal("destination", exp.ParamName);
    }
    #endregion

    [Fact]
    public async Task CalculateAsync_Executes()
    {
        const decimal distance = 1;
        var grid = new Grid();

        var startPosition = new Position
        {
            X = ValidX,
            Y = ValidY
        };

        var endPosition = new Position
        {
            X = ValidX,
            Y = ValidY + 1
        };

        var startItem = new GridItem
        {
            Position = startPosition
        };

        var endItem = new GridItem
        {
            Position = endPosition
        };

        startItem.AddNeighbor(endItem, distance);

        _ = grid.AddItem(startItem);
        _ = grid.AddItem(endItem);

        var subject = new RouteCalculator();
        var result = await subject.CalculateAsync(grid, startPosition, endPosition);

        Assert.NotNull(result);
        Assert.True(result.Any());
    }
}
