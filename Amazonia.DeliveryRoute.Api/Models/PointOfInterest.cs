using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Api.Models;

/// <summary>
/// Denotes a point of interest in the grid
/// </summary>
public class PointOfInterest
    : IEquatable<PointOfInterest>,
    IComparable<PointOfInterest>
{
    #region Properties
    /// <summary>
    /// Qualifies this Point of Interest
    /// </summary>
    [Required]
    public required InterestType? InterestType { get; set; }

    /// <summary>
    /// Locates the Point of Interest in the grid
    /// </summary>
    [Required]
    public required Position Position { get; set; }
    #endregion

    #region Equality
    /// <inheritdoc/>
    public override bool Equals(object? other)
    {
        return this.Equals(other as PointOfInterest);
    }

    /// <inheritdoc/>
    public bool Equals(PointOfInterest? other)
    {
        return object.Equals(this.Position, other?.Position);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.InterestType, this.Position);
    }
    #endregion

    #region Comparable
    /// <inheritdoc/>
    public int CompareTo(PointOfInterest? other)
    {
        return this.Position.CompareTo(other?.Position);
    }
    #endregion
}
