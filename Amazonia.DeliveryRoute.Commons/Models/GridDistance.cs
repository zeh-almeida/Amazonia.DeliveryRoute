using CommunityToolkit.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Defines the distance between two <see cref="GridItem"/>
/// </summary>
public sealed class GridDistance<TValue>
    : IEquatable<GridDistance<TValue>>,
    IComparable<GridDistance<TValue>>
    where TValue : class
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
    public required Vertex<TValue> Other { get; set; }

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
        return this.Equals(other as GridDistance<TValue>);
    }

    /// <inheritdoc/>
    public bool Equals(GridDistance<TValue>? other)
    {
        return Equals(this.Other, other?.Other);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Other);
    }
    #endregion

    #region Comparable
    /// <inheritdoc/>
    public int CompareTo(GridDistance<TValue>? other)
    {
        var result = this.Value.CompareTo(other?.Value);

        if (0.Equals(result))
        {
            result = this.Other.CompareTo(other?.Other);
        }

        return result;
    }
    #endregion

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"({this.Other.Value} | {this.Value:0.0####})";
    }

    /// <summary>
    /// Checks if this distance is related to the desired item
    /// </summary>
    /// <param name="item">Item to check for</param>
    /// <returns>True if the Item is related, false otherwise</returns>
    public bool RelatedTo(Vertex<TValue> item)
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
    public Vertex<TValue>? CoversPosition(TValue position)
    {
        Guard.IsNotNull(position);
        return this.Other.Value.Equals(position) ? this.Other : null;
    }
}
