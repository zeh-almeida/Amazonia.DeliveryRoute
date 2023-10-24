using CommunityToolkit.Diagnostics;
using System.Text.RegularExpressions;

namespace Amazonia.DeliveryRoute.Commons.Extensions;

/// <summary>
/// Provides useful extensions to the String type
/// </summary>
public static partial class StringExtensions
{
    #region Constants
    /// <summary>
    /// Number of letters, from A - Z
    /// </summary>
    private const int AlphabetLength = 26;

    /// <summary>
    /// Validation pattern used to convert strings into column indexes
    /// </summary>
    public const string ValidationPattern = "^[a-zA-Z]*$";
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
        var matched = InputRegex().IsMatch(value);

        if (!matched)
        {
            throw new ArgumentException($"'{value}' does not match the validation pattern", nameof(value));
        }

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

    /// <summary>
    /// Splits the string into its alpha and numeric parts.
    /// </summary>
    /// <param name="value">Value to split</param>
    /// <returns>Alpha and numeric parts of the original string</returns>
    /// <exception cref="ArgumentException">Thrown if value is null or whitespaced</exception>
    public static (string, int) AsCoordinates(this string value)
    {
        Guard.IsNotNullOrWhiteSpace(value);
        var parts = AlphaNumericRegex().Match(value);

        return 2.Equals(parts.Length)
             ? ((string, int))(parts.Groups["Alpha"].Value, Convert.ToInt32(parts.Groups["Numeric"].Value))
             : throw new ArgumentException($"'{value}' must be a coordinate such as 'A2', 'ZZ99', etc", nameof(value));
    }

    [GeneratedRegex(
    ValidationPattern,
    RegexOptions.CultureInvariant
    | RegexOptions.Compiled
    | RegexOptions.Singleline)]
    private static partial Regex InputRegex();

    /// <summary>
    /// Splits the string into its alpha and numeric parts.
    /// </summary>
    /// <returns>"Alpha" and "Numeric" capture groups</returns>
    [GeneratedRegex(
    "(?<Alpha>[A-Z]*)(?<Numeric>[0-9]*)",
    RegexOptions.CultureInvariant
    | RegexOptions.Compiled
    | RegexOptions.Singleline)]
    public static partial Regex AlphaNumericRegex();
}
