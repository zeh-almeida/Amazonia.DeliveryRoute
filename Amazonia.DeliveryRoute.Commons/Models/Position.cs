using Amazonia.DeliveryRoute.Commons.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Amazonia.DeliveryRoute.Commons.Models;

/// <summary>
/// Definition of a Position in the Delivery board for Amazonia
/// </summary>
public sealed record Position
    : IComparable<Position>
{
    #region Constants
    /// <summary>
    /// Maximum value for <see cref="Position.Y"/>
    /// </summary>
    public const int MaxY = 8;

    /// <summary>
    /// Minimum value for <see cref="Position.Y"/>
    /// </summary>
    public const int MinY = 1;
    #endregion

    #region Properties
    /// <summary>
    /// Alpha string which indicates the position in the X-axis.
    /// Must be a single character from A-Z
    /// <see cref="StringExtensions.ValidationPattern"/>
    /// </summary>
    [Required]
    [StringLength(1, MinimumLength = 1)]
    [RegularExpression(StringExtensions.ValidationPattern)]
    public required string X { get; set; }

    /// <summary>
    /// Position in the Y-axis
    /// <see cref="MaxY"/>
    /// <see cref="MinY"/>
    /// </summary>
    [Required]
    [Range(MinY, MaxY)]
    public required int Y { get; set; }
    #endregion

    #region Comparable
    /// <inheritdoc/>
    [SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Ternary makes it harder to read")]
    public int CompareTo(Position? other)
    {
        if (other is null)
        {
            return 1;
        }

        if (this.X.Equals(other.X)
            && this.Y.Equals(other.Y))
        {
            return 0;
        }

        if (this.X.CompareTo(other.X) < 0)
        {
            return -1;
        }

        return this.Y.CompareTo(other.Y) < 0 ? -1 : 1;
    }
    #endregion

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"P({this.X}{this.Y})";
    }
}
