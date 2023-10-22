using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.RouteCalculation.Models;

/// <summary>
/// Result of the <see cref="IRouteCalculator{TValue}"/> service execution
/// </summary>
/// <typeparam name="TValue">Value reference of the result</typeparam>
public sealed record RoutingResult<TValue>
    where TValue : class
{
    #region Properties
    /// <summary>
    /// Path calculated to be the shortest
    /// </summary>
    [Required]
    public IOrderedEnumerable<TValue> Path { get; init; }

    /// <summary>
    /// Total distance between start and finish for the calculated route
    /// </summary>
    [Required]
    public decimal TotalDistance { get; init; }
    #endregion
}
