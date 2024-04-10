using DevToys.Tools.Helpers;

namespace DevToys.Tools.UnitTests.Tools.Helpers;

public class GZipHelperTests
{
    [Theory]
    [InlineData(null, "")]
    [InlineData("", "H4sIAAAAAAAACgMAAAAAAAAAAAA=")]
    [InlineData("  ", "H4sIAAAAAAAAClNQAACVFjPvAgAAAA==")]
    [InlineData("Hello World!", "H4sIAAAAAAAACvNIzcnJVwjPL8pJUQQAoxwpHAwAAAA=")]
    [InlineData("Hello World! é)à", "H4sIAAAAAAAACvNIzcnJVwjPL8pJUVQ4vFLz8AIAeJm72xIAAAA=")]
    [InlineData("H4sIAAAAAAAACvNIzcnJVwjPL8pJUVQ4vFLz8AIAeJm72xIAAAA=", "H4sIAAAAAAAACvMwKfZ0hALnMj/PquQ8r7DyrAAfiwKv0LBAkzI3nyoLR0/HVK9cc6MKsFJbAI7LDrY0AAAA")]
    internal async Task CompressToGZipWindows(string input, string expectedResult)
    {
        if (OperatingSystem.IsWindows())
        {
            (await GZipHelper.CompressGZipDataAsync(
                    input,
                    new MockILogger(),
                    CancellationToken.None))
                .compressedData
                .Should()
                .Be(expectedResult);
        }
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "H4sIAAAAAAAAEwMAAAAAAAAAAAA=")]
    [InlineData("  ", "H4sIAAAAAAAAE1NQAACVFjPvAgAAAA==")]
    [InlineData("Hello World!", "H4sIAAAAAAAAE/NIzcnJVwjPL8pJUQQAoxwpHAwAAAA=")]
    [InlineData("Hello World! é)à", "H4sIAAAAAAAAE/NIzcnJVwjPL8pJUVQ4vFLz8AIAeJm72xIAAAA=")]
    [InlineData("H4sIAAAAAAAACvNIzcnJVwjPL8pJUVQ4vFLz8AIAeJm72xIAAAA=", "H4sIAAAAAAAAE/MwKfZ0hALnMj/PquQ8r7DyrAAfiwKv0LBAkzI3nyoLR0/HVK9cc6MKsFJbAI7LDrY0AAAA")]
    internal async Task CompressToGZipUnix(string input, string expectedResult)
    {
        if (!OperatingSystem.IsWindows())
        {
            (await GZipHelper.CompressGZipDataAsync(
                    input,
                    new MockILogger(),
                    CancellationToken.None))
                .compressedData
                .Should()
                .Be(expectedResult);
        }
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("H4sIAAAAAAAACgMAAAAAAAAAAAA=", "")]
    [InlineData("H4sIAAAAAAAAClNQAACVFjPvAgAAAA==", "  ")]
    [InlineData("H4sIAAAAAAAACvNIzcnJVwjPL8pJUQQAoxwpHAwAAAA=", "Hello World!")]
    [InlineData("H4sIAAAAAAAACvNIzcnJVwjPL8pJUVQ4vFLz8AIAeJm72xIAAAA=", "Hello World! é)à")]
    [InlineData("H4sIAAAAAAAACvMwKfZ0hALnMj/PquQ8r7DyrAAfiwKv0LBAkzI3nyoLR0/HVK9cc6MKsFJbAI7LDrY0AAAA", "H4sIAAAAAAAACvNIzcnJVwjPL8pJUVQ4vFLz8AIAeJm72xIAAAA=")]
    [InlineData("Hello", "<Invalid GZip data>")]
    internal async Task DecompressGZip(string input, string expectedResult)
    {
        (await GZipHelper.DecompressGZipDataAsync(
            input,
            new MockILogger(),
            CancellationToken.None))
            .decompressedData
            .Should()
            .Be(expectedResult);
    }
}
