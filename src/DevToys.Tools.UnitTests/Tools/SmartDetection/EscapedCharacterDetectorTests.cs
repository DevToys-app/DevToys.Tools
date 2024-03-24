using DevToys.Tools.SmartDetection;

namespace DevToys.Tools.UnitTests.Tools.SmartDetection;

public class EscapedCharacterDetectorTests : TestBase
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("hello world", false)]
    [InlineData("hello\rworld", false)]
    [InlineData("hello\\rworld", true)]
    [InlineData("hello\\nworld", true)]
    [InlineData("hello\\\\world", true)]
    [InlineData("hello\\\"world", true)]
    [InlineData("hello\\tworld", true)]
    [InlineData("hello\\fworld", true)]
    [InlineData("hello\\bworld", true)]
    public async Task TryDetectDataAsync(string input, bool expectedResult)
    {
        DataDetectionResult dataDetectionResult = new(false, input);
        EscapedCharactersDetector detector = new();
        DataDetectionResult result = await detector.TryDetectDataAsync(
            input,
            dataDetectionResult,
            default);

        result.Success.Should().Be(expectedResult);
    }
}
