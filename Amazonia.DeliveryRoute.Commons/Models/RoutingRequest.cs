using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Denotes a request from the API to recover the distance between two points
/// </summary>
/// <typeparam name="TValue">Value reference of the request</typeparam>
public sealed record RoutingRequest<TValue>
    where TValue : class
{
    #region Properties
    /// <summary>
    /// Start value
    /// </summary>
    [Required]
    public TValue StartPoint { get; set; }

    /// <summary>
    /// Destination value
    /// </summary>
    [Required]
    public TValue DestinationPoint { get; set; }
    #endregion
}
