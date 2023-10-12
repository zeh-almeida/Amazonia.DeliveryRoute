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
    public static string AsColumnName(this int value)
    {
        var columnName = string.Empty;

        // Copies value in order not to change the parameter
        // avoids unexpected behavior
        var index = value;

        while (index > 0)
        {
            var modulo = (index - 1) % AlphabetLength;

            // 'A' is the first letter so count must start from there
            columnName = Convert.ToChar('A' + modulo) + columnName;
            index = (index - modulo) / AlphabetLength;
        }

        return columnName;
    }
}
