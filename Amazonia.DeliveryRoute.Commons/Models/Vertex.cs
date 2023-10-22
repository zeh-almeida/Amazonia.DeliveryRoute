using CommunityToolkit.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Item in the delivery Grid connected to other items to form the delivery Matrix
/// </summary>
public sealed class Vertex<TValue>
    : IEquatable<Vertex<TValue>>,
    IComparable<Vertex<TValue>>
    where TValue : class
{
    #region Properties
    /// <summary>
    /// Value of the vertex
    /// </summary>
    [Required]
    public required TValue Value { get; set; }

    /// <summary>
    /// Relation to other Vertices and the distance between them
    /// </summary>
    private SortedSet<GridDistance<TValue>> Neighbors { get; }

    /// <summary>
    /// Calculated distance from the source to this Vertex
    /// </summary>
    public decimal TotalDistance { get; set; } = decimal.MaxValue;

    /// <summary>
    /// Pervious vertex used to navigate to the current one
    /// </summary>
    public Vertex<TValue>? Previous { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Initializes a new Vertex
    /// </summary>
    public Vertex()
    {
        this.Neighbors = [];
    }
    #endregion

    #region Equality
    /// <inheritdoc/>
    public override bool Equals(object? other)
    {
        return this.Equals(other as Vertex<TValue>);
    }

    /// <inheritdoc/>
    public bool Equals(Vertex<TValue>? other)
    {
        return this.Value.Equals(other?.Value);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Value);
    }
    #endregion

    #region Comparable
    /// <inheritdoc/>
    public int CompareTo(Vertex<TValue>? other)
    {
        return Comparer<TValue>.Default.Compare(this.Value, other?.Value);
    }
    #endregion

    /// <inheritdoc/>
    public override string ToString()
    {
        var neighbors = this.Neighbors.Select(x => x.Other.Value);
        return $"({this.Value}: [{string.Join(", ", neighbors)}])";
    }

    /// <summary>
    /// Adds a new neighbor to this Vertex.
    /// If the neighbor already exists, it is not updated.
    /// </summary>
    /// <remarks>The distance must be greater than <see cref="GridDistance{TValue}.MinimalDistance"/></remarks>
    /// <param name="other">Vertex to make as neighbor</param>
    /// <param name="distance">Distance between the vertices</param>
    public void AddNeighbor(Vertex<TValue> other, decimal distance)
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
    /// Checks if the desired vertex is already a neighbor of the current vertex
    /// </summary>
    /// <param name="other">Vertex to check for</param>
    /// <returns>True if already neighbor, false otherwise</returns>
    public bool IsNeighbor(Vertex<TValue> other)
    {
        Guard.IsNotNull(other);
        return this.Neighbors.Any(n => n.RelatedTo(other));
    }

    /// <summary>
    /// Returns a copy of all neighbors for this Vertex
    /// </summary>
    /// <returns>All neighbors for this Vertex</returns>
    public IEnumerable<GridDistance<TValue>> AllNeighbors()
    {
        return this.Neighbors.AsEnumerable();
    }
}
