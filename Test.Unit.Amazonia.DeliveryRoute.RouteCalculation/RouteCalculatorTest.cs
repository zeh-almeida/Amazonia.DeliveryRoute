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
    public async Task Start_EqualsDestination_Throws()
    {
        var position = new Position
        {
            X = ValidX,
            Y = ValidY
        };

        var grid = new Grid<Position>();
        _ = grid.AddItem(new Vertex<Position> { Value = position });

        var subject = new RouteCalculator();
        var exp = await Assert.ThrowsAsync<ArgumentException>(() => subject.CalculateAsync(grid, position, position));

        Assert.NotNull(exp);
        Assert.Equal("destination", exp.ParamName);
    }
    #endregion

    [Fact]
    public async Task CalculateAsync_Executes()
    {
        const decimal distance = 1;

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

        var startItem = new Vertex<Position>
        {
            Value = startPosition
        };

        var endItem = new Vertex<Position>
        {
            Value = endPosition
        };

        startItem.ConnectTo(endItem, distance);

        var grid = new Grid<Position>();
        _ = grid.AddItem(startItem);
        _ = grid.AddItem(endItem);

        var subject = new RouteCalculator();
        var result = await subject.CalculateAsync(grid, startPosition, endPosition);

        Assert.NotNull(result);
        Assert.True(result.Any());
    }
}
