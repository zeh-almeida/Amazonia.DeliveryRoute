using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Defines the distance between two <see cref="Vertex{TValue}"/>
/// </summary>
public sealed class VertexConnection<TValue>
    : IEquatable<VertexConnection<TValue>>,
    IComparable<VertexConnection<TValue>>
    where TValue : class
{
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
        return this.Equals(other as VertexConnection<TValue>);
    }

    /// <inheritdoc/>
    public bool Equals(VertexConnection<TValue>? other)
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
    public int CompareTo(VertexConnection<TValue>? other)
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
        return $"C({this.Other.Value} | {this.Value:0.0####})";
    }
}
