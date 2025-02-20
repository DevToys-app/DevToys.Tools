using DevToys.Tools.Tools.Testers.JsonPathTester;

namespace DevToys.Tools.UnitTests.Tools.Testers.JsonPathTester;

public sealed class JsonPathTesterGuiToolTests : TestBase
{
    private readonly JsonPathTesterGuiTool _tool;
    private readonly UIToolView _toolView;
    private readonly IUIMultiLineTextInput _inputBox;
    private readonly IUISingleLineTextInput _jsonPathInputBox;
    private readonly IUIMultiLineTextInput _outputBox;
    private readonly string _baseTestDataDirectory = Path.Combine("Tools", "TestData", nameof(JsonPathTester));

    public JsonPathTesterGuiToolTests()
    {
        _tool = new JsonPathTesterGuiTool(new MockISettingsProvider());

        _toolView = _tool.View;

        _inputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("input-json-json-path-tester");
        _jsonPathInputBox = (IUISingleLineTextInput)_toolView.GetChildElementById("input-json-path-json-path-tester");
        _outputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("output-json-path-tester");
    }

    [Fact]
    public async Task TestJsonPathUiObject()
    {
        string inputJson = await TestDataProvider.GetEmbeddedFileContent("DevToys.Tools.UnitTests.Tools.TestData.JsonPathTester.sample-object.json");

        _inputBox.Text(inputJson);
        _jsonPathInputBox.Text("$.phoneNumbers[:1].type");
        await _tool.WorkTask;

        _outputBox.Text.Should().Be(
            @"[
  ""iPhone""
]");
    }

    [Fact]
    public async Task TestJsonPathUiObjectFailed()
    {
        string inputJson = await TestDataProvider.GetEmbeddedFileContent("DevToys.Tools.UnitTests.Tools.TestData.JsonPathTester.sample-object.json");

        _inputBox.Text(inputJson);
        _jsonPathInputBox.Text("$.TEST");
        await _tool.WorkTask;

        _outputBox.Text.Should().Be("[]");
    }

    [Fact]
    public async Task TestJsonPathUiArray()
    {
        string inputJson = await TestDataProvider.GetEmbeddedFileContent("DevToys.Tools.UnitTests.Tools.TestData.JsonPathTester.sample-array.json");

        _inputBox.Text(inputJson);
        _jsonPathInputBox.Text("$[0].foo");
        await _tool.WorkTask;

        _outputBox.Text.Should().Be(@"[
  1
]");
    }

    [Fact]
    public async Task TestJsonPathUiArrayFailed()
    {
        string inputJson = await TestDataProvider.GetEmbeddedFileContent("DevToys.Tools.UnitTests.Tools.TestData.JsonPathTester.sample-array.json");

        _inputBox.Text(inputJson);
        _jsonPathInputBox.Text("$[0].TEST");
        await _tool.WorkTask;

        _outputBox.Text.Should().Be("[]");
    }
}
