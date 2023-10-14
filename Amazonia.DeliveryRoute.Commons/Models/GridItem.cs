using CommunityToolkit.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Defines a placement in the delivery grid
/// </summary>
public sealed class GridItem
    : IEquatable<GridItem>,
    IComparable<GridItem>
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
    private HashSet<GridDistance> Neighbors { get; }
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
        return this.Equals(other as GridItem);
    }

    /// <inheritdoc/>
    public bool Equals(GridItem? other)
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
    public int CompareTo(GridItem? other)
    {
        return this.Position.CompareTo(other?.Position);
    }
    #endregion

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"({this.Position}: [{string.Join(", ", this.Neighbors)}])";
    }

    /// <summary>
    /// Adds a new neighbor to this Item.
    /// If the neighbor already exists, it is not updated.
    /// </summary>
    /// <remarks>The distance must be greater than <see cref="GridDistance.MinimalDistance"/></remarks>
    /// <param name="other">Item to make as neighbor</param>
    /// <param name="distance">Distance between the items</param>
    public void AddNeighbor(GridItem other, decimal distance)
    {
        Guard.IsNotNull(other);
        Guard.IsGreaterThan(distance, GridDistance.MinimalDistance);

        var value = new GridDistance
        {
            ItemA = this,
            ItemB = other,
            Value = distance,
        };

        _ = this.Neighbors.Add(value);
        _ = other.Neighbors.Add(value);
    }

    /// <summary>
    /// Checks if the desired item is already a neighbor of the current Item
    /// </summary>
    /// <param name="other">Item to check for</param>
    /// <returns>True if already neighbor, false otherwise</returns>
    public bool IsNeighbor(GridItem other)
    {
        Guard.IsNotNull(other);
        return this.Neighbors.Any(n => n.RelatedTo(other));
    }

    /// <summary>
    /// Finds a neighbor based on their position
    /// </summary>
    /// <param name="position">position of the desired neighbor</param>
    /// <returns>Neighbor at the desired position or null if not found</returns>
    public GridItem? FindNeighbor(Position position)
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
    public IEnumerable<GridItem> AllNeighbors()
    {
        return this.Neighbors.Select(n => n.ItemB);
    }
}
