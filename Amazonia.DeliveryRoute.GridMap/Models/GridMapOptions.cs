using System.Diagnostics.CodeAnalysis;

namespace Amazonia.DeliveryRoute.GridMap.Models;

/// <summary>
/// Options for the <see cref="IGridService{TValue}"/> to execute
/// </summary>
[ExcludeFromCodeCoverage]
public sealed record GridMapOptions
{
    #region Constants
    /// <summary>
    /// Configuration section to load from
    /// </summary>
    public const string Section = "Amazonia.DeliveryRoute.GridMap";
    #endregion

    #region Properties
    /// <summary>
    /// Base URI for the Grid API
    /// </summary>
    public string GridSourceBaseUri { get; set; } = string.Empty;

    /// <summary>
    /// Path URI for the Grid API resource
    /// </summary>
    public string GridSourceApiUri { get; set; } = string.Empty;
    #endregion
}
