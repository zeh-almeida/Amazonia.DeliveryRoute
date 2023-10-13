namespace Amazonia.DeliveryRoute.Api.Models;

/// <summary>
/// Types of Interest for objects in the grid
/// </summary>
public enum InterestType
{
    /// <summary>
    /// Denotes the point of interest as the starting position
    /// </summary>
    OriginPosition = 1,

    /// <summary>
    /// Denotes the mid point between origin and destination
    /// </summary>
    ObjectPickup = 2,

    /// <summary>
    /// Final point of interest in the grid
    /// </summary>
    DeliveryDestination = 3,
}
