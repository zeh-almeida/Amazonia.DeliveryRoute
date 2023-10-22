using CommunityToolkit.Diagnostics;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Maintains all known <see cref="Vertex{TValue}"/>s
/// </summary>
public sealed record Grid<TValue>
    where TValue : class
{
    #region Properties
    private HashSet<Vertex<TValue>> Items { get; set; }
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
        return $"G([{string.Join(", ", this.Items)}])";
    }

    /// <summary>
    /// Adds a new Item to the Grid
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <returns>True if added, false if already known</returns>
    public bool AddItem(Vertex<TValue> item)
    {
        Guard.IsNotNull(item);
        return this.Items.Add(item);
    }

    /// <summary>
    /// removes an Item from the Grid
    /// </summary>
    /// <param name="item">Item to remove</param>
    /// <returns>True if removed, false if not found</returns>
    public bool RemoveItem(Vertex<TValue> item)
    {
        Guard.IsNotNull(item);
        return this.Items.Remove(item);
    }

    /// <summary>
    /// Finds an item at the grid based on its value
    /// </summary>
    /// <param name="value">Value to search for</param>
    /// <returns>Item with the desired value or null if it doesn't exist</returns>
    public Vertex<TValue>? FindItem(TValue value)
    {
        return this.Items.FirstOrDefault(item => item.Value.Equals(value));
    }

    /// <summary>
    /// Enumerates all current items
    /// </summary>
    /// <returns>All current items</returns>
    public IEnumerable<Vertex<TValue>> AsEnumerable()
    {
        return this.Items;
    }
}
