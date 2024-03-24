using DevToys.Tools.Tools.EncodersDecoders.Html;

namespace DevToys.Tools.UnitTests.Tools.EncodersDecoders.Html;

public sealed class HtmlEncoderDecoderGuiToolTests : TestBase
{
    private readonly HtmlEncoderDecoderGuiTool _tool;
    private readonly UIToolView _toolView;
    private readonly IUIMultiLineTextInput _inputBox;
    private readonly IUIMultiLineTextInput _outputBox;

    public HtmlEncoderDecoderGuiToolTests()
    {
        _tool = new HtmlEncoderDecoderGuiTool(new MockISettingsProvider());

        _toolView = _tool.View;

        _inputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("html-input-box");
        _outputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("html-output-box");
    }

    [Fact]
    public async Task SwitchConversionMode()
    {
        var conversionMode = (IUISwitch)_toolView.GetChildElementById("html-conversion-mode-switch");

        _inputBox.Text("<hello>");
        await _tool.WorkTask;
        _outputBox.Text.Should().Be("&lt;hello&gt;");

        conversionMode.Off(); // Switch to Decode

        await _tool.WorkTask;
        _inputBox.Text.Should().Be("&lt;hello&gt;");
        await _tool.WorkTask;
        _outputBox.Text("<hello>");

        conversionMode.On(); // Switch to Encode

        await _tool.WorkTask;
        _inputBox.Text("<hello>");
        await _tool.WorkTask;
        _outputBox.Text.Should().Be("&lt;hello&gt;");
    }
}
