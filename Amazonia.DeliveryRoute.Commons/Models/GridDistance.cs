using CommunityToolkit.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Defines the distance between two <see cref="GridItem"/>
/// </summary>
public sealed class GridDistance
    : IEquatable<GridDistance>
{
    #region Constants
    /// <summary>
    /// Minimal distance: all values should be greater than this.
    /// </summary>
    public const decimal MinimalDistance = 0;
    #endregion

    #region Properties
    /// <summary>
    /// Right neighbor
    /// </summary>
    [Required]
    public required GridItem Other { get; set; }

    /// <summary>
    /// Distance between the neighbors
    /// </summary>
    [Required]
    [Range(1, double.MaxValue)]
    public required decimal Value { get; set; }
    #endregion

    #region Equality
    /// <inheritdoc/>
    public override bool Equals(object? other)
    {
        return this.Equals(other as GridDistance);
    }

    /// <inheritdoc/>
    public bool Equals(GridDistance? other)
    {
        return Equals(this.Other, other?.Other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Other);
    }
    #endregion

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"({this.Other.Position} | {this.Value:0.0####})";
    }

    /// <summary>
    /// Checks if this distance is related to the desired item
    /// </summary>
    /// <param name="item">Item to check for</param>
    /// <returns>True if the Item is related, false otherwise</returns>
    public bool RelatedTo(GridItem item)
    {
        Guard.IsNotNull(item);
        return Equals(this.Other, item);
    }

    /// <summary>
    /// Returns the GridItem at the position for this relationship.
    /// If the position is unknown, returns null.
    /// </summary>
    /// <param name="position">Position to check for</param>
    /// <returns>GridItem at the position for this relationship, null otherwise</returns>
    public GridItem? CoversPosition(Position position)
    {
        Guard.IsNotNull(position);
        return this.Other.Position.Equals(position) ? this.Other : null;
    }
}
