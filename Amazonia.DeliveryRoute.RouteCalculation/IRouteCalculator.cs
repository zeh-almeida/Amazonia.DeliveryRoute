using Amazonia.DeliveryRoute.Commons.Models;

namespace Amazonia.DeliveryRoute.RouteCalculation;

/// <summary>
/// Calculates the distance between two points in a grid
/// </summary>
public interface IRouteCalculator<TValue>
    where TValue : class
{
    /// <summary>
    /// Calculates the route between the start position and the destination
    /// in the desired grid
    /// </summary>
    /// <param name="grid"><see cref="Grid{TValue}"/> containing the points</param>
    /// <param name="start"><see cref="Position"/> in which to start the path tracing</param>
    /// <param name="destination"><see cref="Position"/> in which to end the path tracing</param>
    /// <param name="cancellationToken">Allows cancellation of the execution</param>
    /// <returns>Sorted <see cref="GridItem{TValue}"/> enumeration with the shortest route</returns>
    Task<IOrderedEnumerable<GridItem<TValue>>> CalculateAsync(Grid<TValue> grid, Position start, Position destination, CancellationToken cancellationToken = default);
}
