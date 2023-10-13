using CommunityToolkit.Diagnostics;

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
    /// Left neighbor
    /// </summary>
    public required GridItem ItemA { get; set; }

    /// <summary>
    /// Right neighbor
    /// </summary>
    public required GridItem ItemB { get; set; }

    /// <summary>
    /// Distance between the neighbors
    /// </summary>
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
        return object.Equals(this.ItemA, other?.ItemA)
            && object.Equals(this.ItemB, other?.ItemB);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.ItemA, this.ItemB);
    }
    #endregion

    /// <summary>
    /// Checks if this distance is related to the desired item
    /// </summary>
    /// <param name="item">Item to check for</param>
    /// <returns>True if the Item is related, false otherwise</returns>
    public bool RelatedTo(GridItem item)
    {
        Guard.IsNotNull(item);

        return object.Equals(this.ItemA, item)
            || object.Equals(this.ItemB, item);
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

        if (this.ItemA.Position.Equals(position))
        {
            return this.ItemA;
        }

        if (this.ItemB.Position.Equals(position))
        {
            return this.ItemB;
        }

        return null;
    }
}
