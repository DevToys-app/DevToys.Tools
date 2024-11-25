using DevToys.Tools.Helpers;
using DevToys.Tools.Models;

namespace DevToys.Tools.UnitTests.Tools.Helpers;

public class Base64HelperTests
{
    [Theory]
    [InlineData(null, Base64Encoding.Utf8, false)]
    [InlineData("", Base64Encoding.Utf8, false)]
    [InlineData(" ", Base64Encoding.Utf8, false)]
    [InlineData("aGVsbG8gd29ybGQ=", Base64Encoding.Utf8, true)]
    [InlineData("aGVsbG8gd2f9ybGQ=", Base64Encoding.Utf8, false)]
    [InlineData("SGVsbG8gV29y", Base64Encoding.Utf8, true)]
    [InlineData("SGVsbG8gVa29y", Base64Encoding.Utf8, false)]
    [InlineData("//5oAGUAbABsAG8AIAB3AG8AcgBsAGQA", Base64Encoding.Utf16, true)]
    [InlineData("//5oAGUAbABsAG8AIAB3AG8AcgBsAGQAAAA=", Base64Encoding.Utf8, false)] // NUL
    [InlineData("aGVsbG8gd29ybGTvv70=", Base64Encoding.Utf8, false)] // Invalid Unicode char
    internal void IsValid(string input, Base64Encoding encoding, bool expectedResult)
    {
        Base64Helper.IsBase64DataStrict(input, encoding).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, "", Base64Encoding.Utf8)]
    [InlineData("Hello World!", "SGVsbG8gV29ybGQh", Base64Encoding.Utf8)]
    [InlineData("Hello World! é)à", "SGVsbG8gV29ybGQhIMOpKcOg", Base64Encoding.Utf8)]
    [InlineData("Hello World! é)à", "//5IAGUAbABsAG8AIABXAG8AcgBsAGQAIQAgAOkAKQDgAA==", Base64Encoding.Utf16)]
    [InlineData("Hello World! é)à", "SGVsbG8gV29ybGQhID8pPw==", Base64Encoding.Ascii)]
    internal void FromTextToBase64(string input, string expectedResult, Base64Encoding encoding)
    {
        Base64Helper.FromTextToBase64(
            input,
            encoding,
            new MockILogger(),
            CancellationToken.None)
            .Should()
            .Be(expectedResult);
    }

    [Theory]
    [InlineData(null, "", Base64Encoding.Utf8)]
    [InlineData("SGVsbG8gV29ybGQh", "Hello World!", Base64Encoding.Utf8)]
    [InlineData("SGVsbG8gV29ybGQhIMOpKcOg", "Hello World! é)à", Base64Encoding.Utf8)]
    [InlineData("//5IAGUAbABsAG8AIABXAG8AcgBsAGQAIQAgAOkAKQDgAA==", "Hello World! é)à", Base64Encoding.Utf16)]
    [InlineData("SGVsbG8gV29ybGQhID8pPw==", "Hello World! ?)?", Base64Encoding.Ascii)]
    internal void FromBase64ToText(string input, string expectedResult, Base64Encoding encoding)
    {
        Base64Helper.FromBase64ToText(
            input,
            encoding,
            new MockILogger(),
            CancellationToken.None)
            .Should()
            .Be(expectedResult);
    }
}
