using CommunityToolkit.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Defines a placement in the delivery grid
/// </summary>
public sealed class GridItem<TValue>
    : IEquatable<GridItem<TValue>>,
    IComparable<GridItem<TValue>>
    where TValue : class
{
    #region Properties
    /// <summary>
    /// Locates the item in the grid
    /// </summary>
    [Required]
    public required Position Position { get; set; }

    /// <summary>
    /// Relation to other GridItems and the distance between them
    /// </summary>
    private SortedSet<GridDistance<TValue>> Neighbors { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new GridItem
    /// </summary>
    public GridItem()
    {
        this.Neighbors = [];
    }
    #endregion

    #region Equality
    /// <inheritdoc/>
    public override bool Equals(object? other)
    {
        return this.Equals(other as GridItem<TValue>);
    }

    /// <inheritdoc/>
    public bool Equals(GridItem<TValue>? other)
    {
        return this.Position.Equals(other?.Position);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Position);
    }
    #endregion

    #region Comparable
    /// <inheritdoc/>
    public int CompareTo(GridItem<TValue>? other)
    {
        return this.Position.CompareTo(other?.Position);
    }
    #endregion

    /// <inheritdoc/>
    public override string ToString()
    {
        var neighbors = this.Neighbors.Select(x => x.Other.Position);
        return $"({this.Position}: [{string.Join(", ", neighbors)}])";
    }

    /// <summary>
    /// Adds a new neighbor to this Item.
    /// If the neighbor already exists, it is not updated.
    /// </summary>
    /// <remarks>The distance must be greater than <see cref="GridDistance.MinimalDistance"/></remarks>
    /// <param name="other">Item to make as neighbor</param>
    /// <param name="distance">Distance between the items</param>
    public void AddNeighbor(GridItem<TValue> other, decimal distance)
    {
        Guard.IsNotNull(other);
        Guard.IsGreaterThan(distance, GridDistance<TValue>.MinimalDistance);

        if (this.Neighbors.Any(x => x.Other.Equals(other)))
        {
            return;
        }

        var value = new GridDistance<TValue>
        {
            Other = other,
            Value = distance,
        };

        _ = this.Neighbors.Add(value);
    }

    /// <summary>
    /// Checks if the desired item is already a neighbor of the current Item
    /// </summary>
    /// <param name="other">Item to check for</param>
    /// <returns>True if already neighbor, false otherwise</returns>
    public bool IsNeighbor(GridItem<TValue> other)
    {
        Guard.IsNotNull(other);
        return this.Neighbors.Any(n => n.RelatedTo(other));
    }

    /// <summary>
    /// Finds a neighbor based on their position and returns their value
    /// </summary>
    /// <param name="position">position of the desired neighbor</param>
    /// <returns>Neighbor at the desired position or null if not found</returns>
    public GridItem<TValue>? FindNeighbor(Position position)
    {
        Guard.IsNotNull(position);

        foreach (var item in this.Neighbors)
        {
            var target = item.CoversPosition(position);
            if (target is not null)
            {
                return target;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns a copy of all neighbors for this Item
    /// </summary>
    /// <returns>All neighbors for this Item</returns>
    public IEnumerable<GridDistance<TValue>> AllNeighbors()
    {
        return this.Neighbors.AsEnumerable();
    }
}
