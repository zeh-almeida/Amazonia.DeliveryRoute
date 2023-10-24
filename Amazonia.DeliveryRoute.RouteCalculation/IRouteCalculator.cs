using Amazonia.DeliveryRoute.Commons.Models;

namespace Amazonia.DeliveryRoute.RouteCalculation;

/// <summary>
/// Calculates the distance between two points in a grid
/// </summary>
public interface IRouteCalculator<TValue>
    where TValue : class
{
    /// <summary>
    /// Calculates the route between the start vertex and the destination
    /// in the desired grid
    /// </summary>
    /// <param name="grid"><see cref="Grid{TValue}"/> where all vertice reside</param>
    /// <param name="start">Value in grid in which to start the path tracing</param>
    /// <param name="destination">Value in grid in which to end the path tracing</param>
    /// <param name="cancellationToken">Allows cancellation of the execution</param>
    /// <returns>Sorted value enumeration with the shortest route</returns>
    Task<RoutingResult<TValue>> CalculateAsync(
        Grid<TValue> grid,
        TValue start,
        TValue destination,
        CancellationToken cancellationToken = default);
}
