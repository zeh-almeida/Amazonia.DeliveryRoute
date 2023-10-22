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
    /// <param name="start"><see cref="Vertex{TValue}"/> in which to start the path tracing</param>
    /// <param name="destination"><see cref="Vertex{TValue}"/> in which to end the path tracing</param>
    /// <param name="cancellationToken">Allows cancellation of the execution</param>
    /// <returns>Sorted <see cref="Vertex{TValue}"/> enumeration with the shortest route</returns>
    Task<IOrderedEnumerable<Vertex<TValue>>> CalculateAsync(Vertex<TValue> start, Vertex<TValue> destination, CancellationToken cancellationToken = default);
}
