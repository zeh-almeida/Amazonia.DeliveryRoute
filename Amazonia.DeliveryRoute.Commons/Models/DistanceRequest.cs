using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Denotes a request from the API to recover the distance between two points
/// </summary>
public sealed record DistanceRequest
{
    #region Properties
    /// <summary>
    /// Start <see cref="Position"/>
    /// </summary>
    [Required]
    public Position StartPoint { get; set; }

    /// <summary>
    /// Destination <see cref="Position"/>
    /// </summary>
    [Required]
    public Position DestinationPoint { get; set; }
    #endregion
}
