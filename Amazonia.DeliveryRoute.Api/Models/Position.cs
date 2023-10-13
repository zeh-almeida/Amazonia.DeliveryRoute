using Amazonia.DeliveryRoute.Commons.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Amazonia.DeliveryRoute.Api.Models;

/// <summary>
/// Definition of a Position in the Delivery board for Amazonia
/// </summary>
public sealed record Position
{
    #region Constants
    /// <summary>
    /// Maximum value for <see cref="Position.Y"/>
    /// </summary>
    public const int MaxY = 7;

    /// <summary>
    /// Minimum value for <see cref="Position.Y"/>
    /// </summary>
    public const int MinY = 0;
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
}
