using CommunityToolkit.Diagnostics;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Maintains all known <see cref="GridItem"/>s
/// </summary>
public sealed record Grid
{
    #region Properties
    private HashSet<GridItem> Items { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Instantiates a new Grid
    /// </summary>
    public Grid()
    {
        this.Items = [];
    }
    #endregion

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[{string.Join(", ", this.Items.Select(i => i.Position).Order())}]";
    }

    /// <summary>
    /// Adds a new Item to the Grid
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <returns>True if added, false if already known</returns>
    public bool AddItem(GridItem item)
    {
        Guard.IsNotNull(item);
        return this.Items.Add(item);
    }

    /// <summary>
    /// removes an Item from the Grid
    /// </summary>
    /// <param name="item">Item to remove</param>
    /// <returns>True if removed, false if not found</returns>
    public bool RemoveItem(GridItem item)
    {
        Guard.IsNotNull(item);
        return this.Items.Remove(item);
    }

    /// <summary>
    /// Finds an item at the grid based on its position
    /// </summary>
    /// <param name="position">Position to search for</param>
    /// <returns>Item at the desired position or null if it doesn't exist</returns>
    public GridItem? FindItem(Position position)
    {
        Guard.IsNotNull(position);
        return this.Items.FirstOrDefault(item => item.Position.Equals(position));
    }

    /// <summary>
    /// Enumerates all current items
    /// </summary>
    /// <returns>All current items</returns>
    public IEnumerable<GridItem> AsEnumerable()
    {
        return this.Items;
    }

    /// <summary>
    /// Amount of items in the grid
    /// </summary>
    /// <returns>Amount of items in the grid</returns>
    public int Count()
    {
        return this.Items.Count;
    }

    /// <summary>
    /// Checks if the grid has any items
    /// </summary>
    /// <returns>True if grid has no items, false otherwise</returns>
    public bool IsEmpty()
    {
        return !this.Items.Any();
    }
}
