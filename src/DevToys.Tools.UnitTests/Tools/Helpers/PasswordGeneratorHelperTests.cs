using DevToys.Tools.Helpers;

namespace DevToys.Tools.UnitTests.Tools.Helpers;

public class PasswordGeneratorHelperTests
{
    [Theory]
    [InlineData(1, true, false, false, false, null)]
    [InlineData(1, false, true, false, false, null)]
    [InlineData(1, false, false, true, false, null)]
    [InlineData(1, false, false, false, true, null)]
    [InlineData(500, true, true, true, true, null)]
    [InlineData(500, true, true, true, true, "bcdefghijklmnopqrstuvwxyz")]
    [InlineData(500, false, true, false, false, "bcdefghijklmnopqrstuvwxyz")]
    [InlineData(500, true, true, true, true, "@, :, /, %, &, ?, #, +, !, $, ^, *")] // https://github.com/DevToys-app/DevToys/issues/1663
    [InlineData(500, true, true, true, true, "^, *")] // https://github.com/DevToys-app/DevToys/issues/1663
    internal void GeneratePassword(int length, bool hasUppercase, bool hasLowercase, bool hasNumber, bool hasSpecialCharacters, string excludedCharacters)
    {
        string password
            = PasswordGeneratorHelper.GeneratePassword(
                length,
                hasUppercase,
                hasLowercase,
                hasNumber,
                hasSpecialCharacters,
                excludedCharacters?.ToCharArray());

        password.Should().NotBeNullOrEmpty();
        password.Length.Should().Be(length);

        if (hasUppercase)
        {
            ContainAny(password, PasswordGeneratorHelper.UppercaseLetters);
        }
        else
        {
            NotContainAny(password, PasswordGeneratorHelper.UppercaseLetters);
        }

        if (hasLowercase)
        {
            ContainAny(password, PasswordGeneratorHelper.LowercaseLetters);
        }
        else
        {
            NotContainAny(password, PasswordGeneratorHelper.LowercaseLetters);
        }

        if (hasNumber)
        {
            ContainAny(password, PasswordGeneratorHelper.Digits);
        }
        else
        {
            NotContainAny(password, PasswordGeneratorHelper.Digits);
        }

        if (hasSpecialCharacters)
        {
            ContainAny(password, PasswordGeneratorHelper.NonAlphanumeric);
        }
        else
        {
            NotContainAny(password, PasswordGeneratorHelper.NonAlphanumeric);
        }

        if (excludedCharacters != null)
            NotContainAny(password, excludedCharacters);
    }

    [Fact]
    internal void GeneratePasswordWithAllCharactersExcludedReturnsEmpty()
    {
        const string allCharacters
            = PasswordGeneratorHelper.UppercaseLetters
            + PasswordGeneratorHelper.LowercaseLetters
            + PasswordGeneratorHelper.Digits
            + PasswordGeneratorHelper.NonAlphanumeric;

        string password
            = PasswordGeneratorHelper.GeneratePassword(
                36,
                hasUppercase: true,
                hasLowercase: true,
                hasNumbers: true,
                hasSpecialCharacters: true,
                allCharacters.ToCharArray());

        password.Should().BeEmpty();
    }

    [Theory]
    [InlineData(true, true, true, true, null, true)]
    [InlineData(true, true, true, true, "@, :, /, %, &, ?, #, +, !, $, ^, *", true)]
    [InlineData(true, true, true, true, "^, *", true)]
    [InlineData(false, false, false, true, PasswordGeneratorHelper.NonAlphanumeric, false)]
    [InlineData(false, true, false, false, PasswordGeneratorHelper.LowercaseLetters, false)]
    [InlineData(
        true,
        true,
        true,
        true,
        PasswordGeneratorHelper.UppercaseLetters
            + PasswordGeneratorHelper.LowercaseLetters
            + PasswordGeneratorHelper.Digits
            + PasswordGeneratorHelper.NonAlphanumeric,
        false)]
    internal void HasAnyCharacterAvailable(bool hasUppercase, bool hasLowercase, bool hasNumbers, bool hasSpecialCharacters, string excludedCharacters, bool expectedResult)
    {
        bool result
            = PasswordGeneratorHelper.HasAnyCharacterAvailable(
                hasUppercase,
                hasLowercase,
                hasNumbers,
                hasSpecialCharacters,
                excludedCharacters?.ToCharArray());

        result.Should().Be(expectedResult);
    }

    private static void ContainAny(string password, string characters)
    {
        characters.Should().ContainAny(password.ToCharArray().Select(c => c.ToString()));
    }

    private static void NotContainAny(string password, string characters)
    {
        characters.Should().NotContainAny(password.ToCharArray().Select(c => c.ToString()));
    }
}
