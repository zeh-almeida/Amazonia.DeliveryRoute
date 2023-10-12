using CommunityToolkit.Diagnostics;

namespace Amazonia.DeliveryRoute.Commons.Extensions;

/// <summary>
/// Provides useful extensions to the String type
/// </summary>
public static class StringExtensions
{
    #region Constants
    /// <summary>
    /// Number of letters, from A - Z
    /// </summary>
    public const int AlphabetLength = 26;
    #endregion

    /// <summary>
    /// Converts a string to a column index
    /// where A = 0, B = 1, etc.
    /// </summary>
    /// <remarks>Only characters from A-Z are valid</remarks>
    /// <param name="value">Value to convert from</param>
    /// <returns>Index of the value</returns>
    public static int AsColumnIndex(this string value)
    {
        Guard.IsNotNullOrWhiteSpace(value);

        var index = 0;
        var chars = value.ToUpperInvariant().ToCharArray();

        foreach (var c in chars)
        {
            index *= AlphabetLength;
            // In ASCII the letter 'A' is not zero, it is actually 65
            index += c - 'A' + 1;
        }

        // A = 0 so we must decrease by one
        return index - 1;
    }
}
