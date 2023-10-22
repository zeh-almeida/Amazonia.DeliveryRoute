using Amazonia.DeliveryRoute.Commons.Models;

namespace Amazonia.DeliveryRoute.GridMap;

/// <summary>
/// Allows for Grid manipulations
/// </summary>
public interface IGridService<TValue>
    where TValue : class
{
    /// <summary>
    /// Builds a new Grid asynchonously
    /// </summary>
    /// <returns>Built Grid task</returns>
    Task<Grid<TValue>> BuildGridAsync(CancellationToken cancellationToken = default);
}
