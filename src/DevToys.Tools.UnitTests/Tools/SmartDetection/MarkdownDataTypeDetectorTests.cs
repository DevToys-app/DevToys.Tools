using DevToys.Tools.SmartDetection;

namespace DevToys.Tools.UnitTests.Tools.SmartDetection;

public class MarkdownDataTypeDetectorTests : TestBase
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("{ \"title\": \"example glossary\" }", false)]
    [InlineData("[ \"title\", \"example glossary\" ]", false)]
    [InlineData("Some random text", false)]
    [InlineData("# Header 1", true)]
    [InlineData("#####  Header 5", true)]
    [InlineData("Some **bold** text", true)]
    [InlineData("Some *italic* text", true)]
    [InlineData("Some text with a [link](https://www.youtube.com/watch?v=dQw4w9WgXcQ&pp=ygUJcmljayByb2xs)", true)]
    [InlineData("![logo](logo.png)", true)]
    [InlineData("| name | description |", true)]
    [InlineData("| name | description |\n|-|-|\n| DevToys.Tools | A set of offline tools installed by default with DevToys. |", true)]
    public async Task TryDetectDataAsync(string input, bool expectedResult)
    {
        DataDetectionResult dataDetectionResult = new(false, input);
        MarkdownDataTypeDetector sut = new();
        DataDetectionResult result = await sut.TryDetectDataAsync(
            input,
            dataDetectionResult,
            default);

        result.Success.Should().Be(expectedResult);
    }
}
