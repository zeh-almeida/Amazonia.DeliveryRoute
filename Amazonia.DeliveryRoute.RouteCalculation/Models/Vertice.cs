namespace Amazonia.DeliveryRoute.RouteCalculation.Models;

internal sealed class Vertice<TValue>
    : IEquatable<Vertice<TValue>>
    where TValue : class
{
    #region Properties
    public required TValue Value { get; set; }

    public Vertice<TValue>? Previous { get; set; }

    public decimal Distance { get; set; } = decimal.MaxValue;

    public decimal Weight { get; set; } = decimal.MaxValue;

    public List<Vertice<TValue>> Neighbors { get; } = [];
    #endregion

    #region Equality
    /// <inheritdoc/>
    public override bool Equals(object? other)
    {
        return this.Equals(other as Vertice<TValue>);
    }

    /// <inheritdoc/>
    public bool Equals(Vertice<TValue>? other)
    {
        return Equals(this.Value, other?.Value);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Value);
    }
    #endregion

    #region Comparable
    /// <inheritdoc/>
    public int CompareTo(Vertice<TValue>? other)
    {
        return Comparer<TValue>.Default.Compare(this.Value, other?.Value);
    }
    #endregion

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"V({this.Value})";
    }
}
