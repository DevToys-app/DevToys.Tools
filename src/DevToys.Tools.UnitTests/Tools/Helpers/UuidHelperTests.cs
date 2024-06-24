using System.Text.RegularExpressions;
using DevToys.Tools.Helpers;
using DevToys.Tools.Models;

namespace DevToys.Tools.UnitTests.Tools.Helpers;

public class UuidHelperTests
{
    [Theory]
    [InlineData(UuidVersion.One, true, true, 1)]
    [InlineData(UuidVersion.One, false, true, 1)]
    [InlineData(UuidVersion.One, true, false, 1)]
    [InlineData(UuidVersion.One, false, false, 1)]
    [InlineData(UuidVersion.Four, true, true, 4)]
    [InlineData(UuidVersion.Four, false, true, 4)]
    [InlineData(UuidVersion.Four, true, false, 4)]
    [InlineData(UuidVersion.Four, false, false, 4)]
    [InlineData(UuidVersion.Seven, true, true, 7)]
    [InlineData(UuidVersion.Seven, false, true, 7)]
    [InlineData(UuidVersion.Seven, true, false, 7)]
    [InlineData(UuidVersion.Seven, false, false, 7)]
    internal void GenerateUuid(UuidVersion uuidVersion, bool hyphens, bool uppercase, int version)
    {
        string newUuid
            = UuidHelper.GenerateUuid(
                uuidVersion,
                hyphens,
                uppercase);
        TestUuid(newUuid, hyphens, uppercase, version);
    }

    private static void TestUuid(string uuid, bool hyphens, bool uppercase, int version)
    {
        Guid.TryParse(uuid, out _).Should().BeTrue();
        uuid.Contains('-').Should().Be(hyphens);
        uuid.Length.Should().Be(hyphens ? 36 : 32);
        uuid.Should().Be(uppercase ? uuid.ToUpperInvariant() : uuid.ToLowerInvariant());
        string pattern = "^[0-9a-f]{8}-?[0-9a-f]{4}-?" + version + "[0-9a-f]{3}-?[89ab][0-9a-f]{3}-?[0-9a-f]{12}$";
        Regex.IsMatch(uuid.ToLowerInvariant(), pattern).Should().BeTrue();
    }
}
