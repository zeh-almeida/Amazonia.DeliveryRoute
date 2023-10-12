namespace Amazonia.DeliveryRoute.Commons.Extensions;

/// <summary>
/// Provides useful extensions to the Int32 type
/// </summary>
public static class IntegerExtensions
{
    #region Constants
    /// <summary>
    /// Number of letters, from A - Z
    /// </summary>
    public const int AlphabetLength = 26;
    #endregion

    /// <summary>
    /// Converts a number to upper-case alpha-representation
    /// where A = 0, B = 1, etc.
    /// </summary>
    /// <param name="value">Value to convert from</param>
    /// <returns>Alpha index of the value</returns>
    /// <see href="https://stackoverflow.com/questions/181596/how-to-convert-a-column-number-e-g-127-into-an-excel-column-e-g-aa"/>
    public static string AsColumnName(this int value)
    {
        const byte baseValue = 'Z' - 'A' + 1;
        var columnName = string.Empty;

        // Copies value in order not to change the parameter
        // avoids unexpected behavior
        var index = value;

        do
        {
            // 'A' is the first letter so count must start from there
            // In ASCII the letter 'A' is not zero, it is actually 65
            columnName = Convert.ToChar('A' + (index % baseValue)) + columnName;
            index = (index / baseValue) - 1;
        } while (index >= 0);

        return columnName;
    }
}
