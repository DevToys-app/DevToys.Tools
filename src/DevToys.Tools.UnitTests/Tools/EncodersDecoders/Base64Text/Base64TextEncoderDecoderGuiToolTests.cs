using DevToys.Tools.Tools.EncodersDecoders.Base64Text;

namespace DevToys.Tools.UnitTests.Tools.EncodersDecoders.Base64Text;

public sealed class Base64TextEncoderDecoderGuiToolTests : TestBase
{
    private readonly Base64TextEncoderDecoderGuiTool _tool;
    private readonly UIToolView _toolView;
    private readonly IUIMultiLineTextInput _inputBox;
    private readonly IUIMultiLineTextInput _outputBox;

    public Base64TextEncoderDecoderGuiToolTests()
    {
        _tool = new Base64TextEncoderDecoderGuiTool(new MockISettingsProvider());

        _toolView = _tool.View;

        _inputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("base64-text-input-box");
        _outputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("base64-text-output-box");
    }

    [Fact]
    public async Task EncodeUtf8()
    {
        _inputBox.Text($"Hello world &é'(-è_çèà){Environment.NewLine}Hello world &é'(-è_çèà)");
        await _tool.WorkTask;
        _outputBox.Text.Should().Be("SGVsbG8gd29ybGQgJsOpJygtw6hfw6fDqMOgKQ0KSGVsbG8gd29ybGQgJsOpJygtw6hfw6fDqMOgKQ==");
    }

    [Fact]
    public async Task EncodeAscii()
    {
        var encodingOption = (IUISelectDropDownList)((IUISetting)_toolView.GetChildElementById("base64-text-encoding-setting")).InteractiveElement;
        encodingOption.Select(1); // Select ASCII

        _inputBox.Text($"Hello world &é'(-è_çèà){Environment.NewLine}Hello world &é'(-è_çèà)");
        await _tool.WorkTask;
        _outputBox.Text.Should().Be("SGVsbG8gd29ybGQgJj8nKC0/Xz8/PykNCkhlbGxvIHdvcmxkICY/JygtP18/Pz8p");
    }

    [Fact]
    public async Task SwitchConversionMode()
    {
        var conversionMode = (IUISwitch)_toolView.GetChildElementById("base64-text-conversion-mode-switch");

        await EncodeUtf8();

        conversionMode.Off(); // Switch to Decode

        await _tool.WorkTask;
        _inputBox.Text.Should().Be("SGVsbG8gd29ybGQgJsOpJygtw6hfw6fDqMOgKQ0KSGVsbG8gd29ybGQgJsOpJygtw6hfw6fDqMOgKQ==");
        await _tool.WorkTask;
        _outputBox.Text("Hello world &é'(-è_çèà)");

        conversionMode.On(); // Switch to Encode
        await _tool.WorkTask;
        await EncodeUtf8();
    }

    [Fact]
    public async Task SwitchMultilineMode()
    {
        var multilineMode = (IUISwitch)_toolView.GetChildElementById("base64-text-multiline-mode-switch");

        await EncodeUtf8();

        multilineMode.On();

        await _tool.WorkTask;
        _outputBox.Text.Should().Be($"SGVsbG8gd29ybGQgJsOpJygtw6hfw6fDqMOgKQ=={Environment.NewLine}SGVsbG8gd29ybGQgJsOpJygtw6hfw6fDqMOgKQ==");
        await _tool.WorkTask;

        multilineMode.Off();
        await _tool.WorkTask;
        _outputBox.Text.Should().Be("SGVsbG8gd29ybGQgJsOpJygtw6hfw6fDqMOgKQ0KSGVsbG8gd29ybGQgJsOpJygtw6hfw6fDqMOgKQ==");

        await EncodeUtf8();
    }
}
