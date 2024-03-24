using DevToys.Tools.Tools.Testers.JsonPathTester;

namespace DevToys.Tools.UnitTests.Tools.Testers.JsonPathTester;

public sealed class JsonPathTesterCommandLineToolTests : TestBase
{
    private readonly JsonPathTesterCommandLineTool _tool;

    public JsonPathTesterCommandLineToolTests()
    {
        _tool = new JsonPathTesterCommandLineTool();
        _tool._fileStorage = new MockIFileStorage();
    }

    [Fact]
    public async Task TestJsonPathUi()
    {
        string inputJson = await TestDataProvider.GetEmbeddedFileContent("DevToys.Tools.UnitTests.Tools.TestData.JsonPathTester.sample.json");

        _tool.InputJson = inputJson;
        _tool.InputJsonPath = "$.phoneNumbers[:1].type";

        using var consoleOutput = new ConsoleOutputMonitor();
        await _tool.InvokeAsync(new MockILogger(), CancellationToken.None);

        string result = consoleOutput.GetOutput().Trim();
        result.Should().Be(
            @"[
  ""iPhone""
]");
    }

    [Fact]
    public async Task TestJsonPathUiFailed()
    {
        string inputJson = await TestDataProvider.GetEmbeddedFileContent("DevToys.Tools.UnitTests.Tools.TestData.JsonPathTester.sample.json");

        _tool.InputJson = inputJson;
        _tool.InputJsonPath = "$.TEST";

        using var consoleOutput = new ConsoleOutputMonitor();
        await _tool.InvokeAsync(new MockILogger(), CancellationToken.None);

        string result = consoleOutput.GetOutput().Trim();
        result.Should().Be("[]");
    }
}
