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
    private SortedSet<VertexConnection<TValue>> Neighbors { get; }

    /// <summary>
    /// Calculated distance from the source to this Vertex
    /// </summary>
    public decimal TotalDistance { get; set; } = decimal.MaxValue;

    /// <summary>
    /// Pervious vertex used to navigate to the current one
    /// </summary>
    public Vertex<TValue>? Previous { get; set; }

    /// <summary>
    /// Returns a copy of all neighbors for this Vertex
    /// </summary>
    public IEnumerable<VertexConnection<TValue>> Connections => this.Neighbors.AsEnumerable();
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
        return $"V({this.Value})";
    }

    #region Connections
    /// <summary>
    /// Connects this Vertex to another.
    /// If the connection already exists, it is not updated.
    /// </summary>
    /// <remarks>The distance must be greater than <see cref="Constants.MinimalDistance"/></remarks>
    /// <param name="other">Vertex to connect to</param>
    /// <param name="distance">Distance between the vertices</param>
    public void ConnectTo(Vertex<TValue> other, decimal distance)
    {
        Guard.IsNotNull(other);
        Guard.IsGreaterThan(distance, Constants.MinimalDistance);

        if (this.Neighbors.Any(x => x.Other.Equals(other)))
        {
            return;
        }

        var value = new VertexConnection<TValue>
        {
            Other = other,
            Value = distance,
        };

        _ = this.Neighbors.Add(value);
    }
    #endregion
}
