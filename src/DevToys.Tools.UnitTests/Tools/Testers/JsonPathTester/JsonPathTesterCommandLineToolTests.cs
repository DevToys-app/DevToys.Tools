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
    public async Task TestJsonPathUiObject()
    {
        string inputJson = await TestDataProvider.GetEmbeddedFileContent("DevToys.Tools.UnitTests.Tools.TestData.JsonPathTester.sample-object.json");

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
    public async Task TestJsonPathUiObjectFailed()
    {
        string inputJson = await TestDataProvider.GetEmbeddedFileContent("DevToys.Tools.UnitTests.Tools.TestData.JsonPathTester.sample-object.json");

        _tool.InputJson = inputJson;
        _tool.InputJsonPath = "$.TEST";

        using var consoleOutput = new ConsoleOutputMonitor();
        await _tool.InvokeAsync(new MockILogger(), CancellationToken.None);

        string result = consoleOutput.GetOutput().Trim();
        result.Should().Be("[]");
    }

    [Fact]
    public async Task TestJsonPathUiArray()
    {
        string inputJson = await TestDataProvider.GetEmbeddedFileContent("DevToys.Tools.UnitTests.Tools.TestData.JsonPathTester.sample-array.json");

        _tool.InputJson = inputJson;
        _tool.InputJsonPath = "$[0].foo";

        using var consoleOutput = new ConsoleOutputMonitor();
        await _tool.InvokeAsync(new MockILogger(), CancellationToken.None);

        string result = consoleOutput.GetOutput().Trim();
        result.Should().Be(@"[
  1
]");
    }

    [Fact]
    public async Task TestJsonPathUiArrayFailed()
    {
        string inputJson = await TestDataProvider.GetEmbeddedFileContent("DevToys.Tools.UnitTests.Tools.TestData.JsonPathTester.sample-array.json");

        _tool.InputJson = inputJson;
        _tool.InputJsonPath = "$[0].TEST";

        using var consoleOutput = new ConsoleOutputMonitor();
        await _tool.InvokeAsync(new MockILogger(), CancellationToken.None);

        string result = consoleOutput.GetOutput().Trim();
        result.Should().Be("[]");
    }
}
