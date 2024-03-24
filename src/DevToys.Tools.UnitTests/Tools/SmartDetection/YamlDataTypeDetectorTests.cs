using DevToys.Tools.SmartDetection;

namespace DevToys.Tools.UnitTests.Tools.SmartDetection;

public class YamlDataTypeDetectorTests : TestBase
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("{ \"title\": \"example glossary\" }", false)]
    [InlineData("[ \"title\", \"example glossary\" ]", false)]
    [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", false)]
    [InlineData("foo :\n  bar :\n    - boo: 1\n    - rab: 2\n    - plop: 3", true)]
    public async Task TryDetectDataAsync(string input, bool expectedResult)
    {
        DataDetectionResult dataDetectionResult = new(false, input);
        YamlDataTypeDetector sut = new();
        DataDetectionResult result = await sut.TryDetectDataAsync(
            input,
            dataDetectionResult,
            default);

        result.Success.Should().Be(expectedResult);
    }
}
