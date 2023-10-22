﻿using Amazonia.DeliveryRoute.Commons.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Amazonia.DeliveryRoute.Api.Models;

/// <summary>
/// Denotes a point of interest in the grid
/// </summary>
public class PointOfInterest
    : IEquatable<PointOfInterest>,
    IEquatable<GridItem<string>>,
    IComparable<PointOfInterest>,
    IComparable<GridItem<string>>
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
    [SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Ternary makes it harder to read")]
    public override bool Equals(object? other)
    {
        if (other is PointOfInterest)
        {
            return this.Equals(other as PointOfInterest);
        }

        if (other is GridItem<string>)
        {
            return this.Equals(other as GridItem<string>);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(PointOfInterest? other)
    {
        return object.Equals(this.Position, other?.Position);
    }

    /// <inheritdoc/>
    public bool Equals(GridItem<string>? other)
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
    public int CompareTo(GridItem<string>? other)
    {
        return this.Position.CompareTo(other?.Position);
    }
    #endregion
}
