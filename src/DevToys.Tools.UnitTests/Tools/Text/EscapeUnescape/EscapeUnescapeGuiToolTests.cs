using DevToys.Tools.Tools.Text.EscapeUnescape;

namespace DevToys.Tools.UnitTests.Tools.Text.EscapeUnescape;

public sealed class EscapeUnescapeGuiToolTests : TestBase
{
    private readonly EscapeUnescapeGuiTool _tool;
    private readonly UIToolView _toolView;
    private readonly IUIMultiLineTextInput _inputBox;
    private readonly IUIMultiLineTextInput _outputBox;

    public EscapeUnescapeGuiToolTests()
    {
        _tool = new EscapeUnescapeGuiTool(new MockISettingsProvider());

        _toolView = _tool.View;

        _inputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("text-escape-unescape-input-box");
        _outputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("text-escape-unescape-output-box");
    }

    [Fact]
    public async Task SwitchConversionMode()
    {
        var conversionMode = (IUISwitch)_toolView.GetChildElementById("text-escape-unescape-conversion-mode-switch");

        _inputBox.Text("hello\rworld");
        await _tool.WorkTask;
        _outputBox.Text.Should().Be("hello\\rworld");

        conversionMode.Off(); // Switch to Unescape

        await _tool.WorkTask;
        _inputBox.Text.Should().Be("hello\\rworld");
        await _tool.WorkTask;
        _outputBox.Text("hello\rworld");

        conversionMode.On(); // Switch to Escape
        await _tool.WorkTask;

        _inputBox.Text("hello\rworld\r");
        await _tool.WorkTask;
        _outputBox.Text.Should().Be("hello\\rworld\\r");
    }
}
