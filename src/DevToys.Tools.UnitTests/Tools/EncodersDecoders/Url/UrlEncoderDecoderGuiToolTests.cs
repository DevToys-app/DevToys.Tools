using DevToys.Tools.Tools.EncodersDecoders.Url;

namespace DevToys.Tools.UnitTests.Tools.EncodersDecoders.Url;

public sealed class UrlEncoderDecoderGuiToolTests : TestBase
{
    private readonly UrlEncoderDecoderGuiTool _tool;
    private readonly UIToolView _toolView;
    private readonly IUIMultiLineTextInput _inputBox;
    private readonly IUIMultiLineTextInput _outputBox;

    public UrlEncoderDecoderGuiToolTests()
    {
        _tool = new UrlEncoderDecoderGuiTool(new MockISettingsProvider());

        _toolView = _tool.View;

        _inputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("url-input-box");
        _outputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("url-output-box");
    }

    [Fact]
    public async Task SwitchConversionMode()
    {
        var conversionMode = (IUISwitch)_toolView.GetChildElementById("url-conversion-mode-switch");

        _inputBox.Text("<hello world>");
        await _tool.WorkTask;
        _outputBox.Text.Should().Be("%3Chello%20world%3E");

        conversionMode.Off(); // Switch to Decode

        await _tool.WorkTask;
        _inputBox.Text.Should().Be("%3Chello%20world%3E");
        await _tool.WorkTask;
        _outputBox.Text("<hello world>");

        conversionMode.On(); // Switch to Encode

        await _tool.WorkTask;
        _inputBox.Text("<hello world>");
        await _tool.WorkTask;
        _outputBox.Text.Should().Be("%3Chello%20world%3E");
    }
}
