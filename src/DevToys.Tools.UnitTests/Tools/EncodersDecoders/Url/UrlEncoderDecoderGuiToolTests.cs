using DevToys.Tools.Tools.EncodersDecoders.Url;

namespace DevToys.Tools.UnitTests.Tools.EncodersDecoders.Url;

public sealed class UrlEncoderDecoderGuiToolTests : TestBase
{
    private readonly UrlEncoderDecoderGuiTool _tool;
    private readonly UIToolView _toolView;
    private readonly IUIMultiLineTextInput _inputBox;
    private readonly IUIMultiLineTextInput _outputBox;

    private readonly string EncodeMode = "url-conversion-mode-switch";
    private readonly string MultiLineMode = "url-conversion-multiline-switch";

    public UrlEncoderDecoderGuiToolTests()
    {
        _tool = new UrlEncoderDecoderGuiTool(new MockISettingsProvider());

        _toolView = _tool.View;

        _inputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("url-input-box");
        _outputBox = (IUIMultiLineTextInput)_toolView.GetChildElementById("url-output-box");
    }

    [Theory(DisplayName = "Url Encode/Decode - Single Line")]
    [InlineData("hello world", "hello%20world", true)]
    [InlineData("hello\r\nworld\r\n!!", "hello%0D%0Aworld%0D%0A%21%21", true)]
    [InlineData("hello\nworld\n!!", "hello%0Aworld%0A%21%21", true)]
    [InlineData("hello%20world", "hello world", false)]
    [InlineData( "hello%0D%0Aworld%0D%0A%21%21", "hello\r\nworld\r\n!!", false)]
    [InlineData("hello%0Aworld%0A%21%21", "hello\nworld\n!!", false)]
    public async Task SingleLineConversion(
        string input,
        string expectedOutput,
        bool encode)
    {
        var conversionMode = (IUISwitch)_toolView.GetChildElementById(EncodeMode);
        if (encode)
        {
            conversionMode.On();
        }
        else
        {
            conversionMode.Off();
        }
        var multiLineMode = (IUISwitch)_toolView.GetChildElementById(MultiLineMode);
        multiLineMode.Off();

        _inputBox.Text(input);
        await _tool.WorkTask;
        _outputBox.Text.Should().Be(expectedOutput);
    }

    [Theory(DisplayName = "Url Encode/Decode - Multi Line")]
    [InlineData("Hello world", "Hello%20world", true)]
    [InlineData("Hello\r\nworld\r\n!!", "Hello\r\nworld\r\n%21%21", true)]
    [InlineData("Hello%20world", "Hello world", false)]
    [InlineData("Hello\r\nworld\r\n%21%21", "Hello\r\nworld\r\n!!", false)]
    public async Task MultiLineConversion(
        string input,
        string expectedOutput,
        bool encode)
    {
        var conversionMode = (IUISwitch)_toolView.GetChildElementById(EncodeMode);
        if (encode)
        {
            conversionMode.On();
        }
        else
        {
            conversionMode.Off();
        }
        var multiLineMode = (IUISwitch)_toolView.GetChildElementById(MultiLineMode);
        multiLineMode.On();
        _inputBox.Text(input);
        await _tool.WorkTask;
        _outputBox.Text.Should().Be(expectedOutput);
    }
}
