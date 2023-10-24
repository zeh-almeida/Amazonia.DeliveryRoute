using System.Diagnostics.CodeAnalysis;

namespace Amazonia.DeliveryRoute.UserInterface.Models;

/// <summary>
/// Options for the UI to execute
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record DeliveryRouteOptions
{
    #region Constants
    /// <summary>
    /// Configuration section to load from
    /// </summary>
    public const string Section = "Amazonia.DeliveryRoute.UserInterface";

    public const int DefaultGridSize = 8;
    #endregion

    #region Properties
    /// <summary>
    /// Base URI for the Grid API
    /// </summary>
    public string ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// Width of the Delivery Grid
    /// </summary>
    public int GridWidth { get; set; } = DefaultGridSize;

    /// <summary>
    /// Height of the Delivery Grid
    /// </summary>
    public int GridHeight { get; set; } = DefaultGridSize;
    #endregion
}
