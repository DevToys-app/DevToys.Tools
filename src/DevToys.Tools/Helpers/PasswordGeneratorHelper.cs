using System.Text;
using DevToys.Tools.Helpers.Core;

namespace DevToys.Tools.Helpers;

internal static class PasswordGeneratorHelper
{
    /// <summary>
    /// Non-alphanumeric characters. !"#$%&')*+,-.:;=>?@]^_}~
    /// </summary>
    /// <remarks>
    /// Excluded characters. (/<[`{| and \
    /// Although it may be possible to use these characters by changing the way they are generated,
    /// they may not be suitable for use.
    /// </remarks>
    internal const string NonAlphanumeric = "!\"#$%&')*+,-.:;=>?@]^_}~";

    /// <summary>
    /// All lower case ASCII characters.
    /// </summary>
    internal const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// All upper case ASCII characters.
    /// </summary>
    internal const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// All digits.
    /// </summary>
    internal const string Digits = "0123456789";

    internal static string GeneratePassword(
        int length,
        bool hasUppercase,
        bool hasLowercase,
        bool hasNumbers,
        bool hasSpecialCharacters,
        char[]? excludedCharacters)
    {
        if (length <= 0)
        {
            return string.Empty;
        }

        var rand = new CryptoRandom();
        var newPasswordCharacters = new List<char>();

        // Combine all character sets together.
        string randomChars = CombineCharacterSets(hasUppercase, hasLowercase, hasNumbers, hasSpecialCharacters, excludedCharacters);

        // Only continue if the user hasn't excluded everything.
        if (randomChars.Length != 0)
        {
            for (int j = 0; j < length; j++)
            {
                newPasswordCharacters.Insert(rand.Next(0, newPasswordCharacters.Count + 1), randomChars[rand.Next(0, randomChars.Length)]);
            }
        }

        return new string(newPasswordCharacters.ToArray());
    }

    /// <summary>
    /// Indicates whether at least one character remains available to generate a password with,
    /// once the excluded characters are removed from the enabled character sets.
    /// </summary>
    internal static bool HasAnyCharacterAvailable(
        bool hasUppercase,
        bool hasLowercase,
        bool hasNumbers,
        bool hasSpecialCharacters,
        char[]? excludedCharacters)
    {
        return CombineCharacterSets(hasUppercase, hasLowercase, hasNumbers, hasSpecialCharacters, excludedCharacters).Length > 0;
    }

    private static string CombineCharacterSets(
        bool hasUppercase,
        bool hasLowercase,
        bool hasNumbers,
        bool hasSpecialCharacters,
        char[]? excludedCharacters)
    {
        var combinedCharsBuilder = new StringBuilder();

        if (hasUppercase)
        {
            combinedCharsBuilder.Append(RemoveExcludedCharacters(UppercaseLetters, excludedCharacters));
        }

        if (hasLowercase)
        {
            combinedCharsBuilder.Append(RemoveExcludedCharacters(LowercaseLetters, excludedCharacters));
        }

        if (hasNumbers)
        {
            combinedCharsBuilder.Append(RemoveExcludedCharacters(Digits, excludedCharacters));
        }

        if (hasSpecialCharacters)
        {
            combinedCharsBuilder.Append(RemoveExcludedCharacters(NonAlphanumeric, excludedCharacters));
        }

        return combinedCharsBuilder.ToString();
    }

    private static string RemoveExcludedCharacters(string input, char[]? excludedCharacters)
    {
        if (excludedCharacters == null || excludedCharacters.Length == 0)
        {
            return input;
        }

        var excludedSet = new HashSet<char>(excludedCharacters); // HashSet provides a faster lookup than Array.Contains().
        var stringBuilder = new StringBuilder();

        foreach (char c in input)
        {
            if (!excludedSet.Contains(c))
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString();
    }
}
