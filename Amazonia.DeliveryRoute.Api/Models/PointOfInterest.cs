using Amazonia.DeliveryRoute.Commons.Models;
using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Api.Models;

/// <summary>
/// Denotes a point of interest in the grid
/// </summary>
public class PointOfInterest
    : IEquatable<PointOfInterest>,
    IEquatable<GridItem>,
    IComparable<PointOfInterest>,
    IComparable<GridItem>
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
        if (other is PointOfInterest)
        {
            return this.Equals(other as PointOfInterest);
        }

        if (other is GridItem)
        {
            return this.Equals(other as GridItem);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(PointOfInterest? other)
    {
        return object.Equals(this.Position, other?.Position);
    }

    /// <inheritdoc/>
    public bool Equals(GridItem? other)
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

    /// <inheritdoc/>
    public int CompareTo(GridItem? other)
    {
        return this.Position.CompareTo(other?.Position);
    }
    #endregion
}
