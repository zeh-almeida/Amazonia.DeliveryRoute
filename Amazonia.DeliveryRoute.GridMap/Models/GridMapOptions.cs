namespace Amazonia.DeliveryRoute.GridMap.Models;

public sealed record GridMapOptions
{
    #region Constants
    public const string Section = "Amazonia.DeliveryRoute.GridMap";
    #endregion

    #region Properties
    public string GridSourceBaseUri { get; set; } = string.Empty;

    public string GridSourceApiUri { get; set; } = string.Empty;
    #endregion
}
