using DevToys.Tools.Tools.EncodersDecoders.GZip;

namespace DevToys.Tools.UnitTests.Tools.EncodersDecoders.GZip;

public sealed class GZipEncoderDecoderGuiToolTests : TestBase
{
    private readonly GZipEncoderDecoderGuiTool _tool;
    private readonly UIToolView _toolView;
    private readonly IUIMultiLineTextInput _inputBox;
    private readonly IUIMultiLineTextInput _outputBox;

    public GZipEncoderDecoderGuiToolTests()
    {
        _tool = new GZipEncoderDecoderGuiTool(new MockISettingsProvider());

        _toolView = _tool.View;

        _inputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("gzip-input-box");
        _outputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("gzip-output-box");
    }

    [Fact]
    public async Task SwitchConversionMode()
    {
        var conversionMode = (IUISwitch)_toolView.GetChildElementById("gzip-compression-mode-switch");

        _inputBox.Text("<hello world>");
        await _tool.WorkTask;
        if (OperatingSystem.IsWindows())
        {
            _outputBox.Text.Should().Be("H4sIAAAAAAAACrPJSM3JyVcozy/KSbEDAKEb3mcNAAAA");
        }
        else if (OperatingSystem.IsMacOS())
        {
            _outputBox.Text.Should().Be("H4sIAAAAAAAAE7PJSM3JyVcozy/KSbEDAKEb3mcNAAAA");
        }
        else if (OperatingSystem.IsLinux())
        {
            _outputBox.Text.Should().Be("H4sIAAAAAAAAA7PJSM3JyVcozy/KSbEDAKEb3mcNAAAA");
        }

        conversionMode.Off(); // Switch to Decompress

        await _tool.WorkTask;
        _outputBox.Text("<hello world>");

        conversionMode.On(); // Switch to Compress

        await _tool.WorkTask;
        _inputBox.Text("<hello world>");
        await _tool.WorkTask;
        if (OperatingSystem.IsWindows())
        {
            _outputBox.Text.Should().Be("H4sIAAAAAAAACrPJSM3JyVcozy/KSbEDAKEb3mcNAAAA");
        }
        else if (OperatingSystem.IsMacOS())
        {
            _outputBox.Text.Should().Be("H4sIAAAAAAAAE7PJSM3JyVcozy/KSbEDAKEb3mcNAAAA");
        }
        else if (OperatingSystem.IsLinux())
        {
            _outputBox.Text.Should().Be("H4sIAAAAAAAAA7PJSM3JyVcozy/KSbEDAKEb3mcNAAAA");
        }
    }
}
