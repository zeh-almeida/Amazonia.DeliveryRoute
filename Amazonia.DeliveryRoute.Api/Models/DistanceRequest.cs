using Amazonia.DeliveryRoute.Commons.Models;
using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Api.Models;

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
    public required Position StartPoint { get; set; }

    /// <summary>
    /// Destination <see cref="Position"/>
    /// </summary>
    [Required]
    public required Position DestinationPoint { get; set; }
    #endregion
}
