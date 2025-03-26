using DevToys.Tools.Helpers;
using DevToys.Tools.Models;
using Microsoft.Extensions.Logging;

namespace DevToys.Tools.Tools.EncodersDecoders.Url;

[Export(typeof(IGuiTool))]
[Name("UrlEncoderDecoder")]
[ToolDisplayInformation(
    IconFontName = "DevToys-Tools-Icons",
    IconGlyph = '\u0121',
    GroupName = PredefinedCommonToolGroupNames.EncodersDecoders,
    ResourceManagerAssemblyIdentifier = nameof(DevToysToolsResourceManagerAssemblyIdentifier),
    ResourceManagerBaseName = "DevToys.Tools.Tools.EncodersDecoders.Url.UrlEncoderDecoder",
    ShortDisplayTitleResourceName = nameof(UrlEncoderDecoder.ShortDisplayTitle),
    LongDisplayTitleResourceName = nameof(UrlEncoderDecoder.LongDisplayTitle),
    DescriptionResourceName = nameof(UrlEncoderDecoder.Description),
    AccessibleNameResourceName = nameof(UrlEncoderDecoder.AccessibleName))]
internal sealed partial class UrlEncoderDecoderGuiTool : IGuiTool, IDisposable
{
    /// <summary>
    /// Whether the tool should encode or decode Url.
    /// </summary>
    private static readonly SettingDefinition<EncodingConversion> conversionMode
        = new(
            name: $"{nameof(UrlEncoderDecoderGuiTool)}.{nameof(conversionMode)}",
            defaultValue: EncodingConversion.Encode);

    private static readonly SettingDefinition<bool> multilineMode
        = new(
            name: $"{nameof(UrlEncoderDecoderGuiTool)}.{nameof(multilineMode)}",
            defaultValue: false);

    private enum GridRows
    {
        Settings,
        Input,
        Output
    }

    private enum GridColumns
    {
        Stretch
    }

    private readonly DisposableSemaphore _semaphore = new();
    private readonly ILogger _logger;
    private readonly ISettingsProvider _settingsProvider;
    private readonly IUISwitch _conversionModeSwitch = Switch("url-conversion-mode-switch");
    private readonly IUISwitch _multilineModeSwitch = Switch("url-conversion-multiple-switch");
    private readonly IUIMultiLineTextInput _inputText = MultiLineTextInput("url-input-box");
    private readonly IUIMultiLineTextInput _outputText = MultiLineTextInput("url-output-box");

    private CancellationTokenSource? _cancellationTokenSource;

    [ImportingConstructor]
    public UrlEncoderDecoderGuiTool(ISettingsProvider settingsProvider)
    {
        _logger = this.Log();
        _settingsProvider = settingsProvider;

        if (_settingsProvider.GetSetting(multilineMode))
        {
            _multilineModeSwitch.On();
        }
        else
        {
            _multilineModeSwitch.Off();
        }

        switch (_settingsProvider.GetSetting(conversionMode))
        {
            case EncodingConversion.Encode:
                _conversionModeSwitch.On();
                _inputText.AutoWrap();
                _outputText.AlwaysWrap();
                break;

            case EncodingConversion.Decode:
                _inputText.AlwaysWrap();
                _outputText.AutoWrap();
                break;

            default:
                throw new NotSupportedException();
        }
    }

    // For unit tests.
    internal Task? WorkTask { get; private set; }

    public UIToolView View
        => new(
            isScrollable: true,
            Grid()
                .RowLargeSpacing()

                .Rows(
                    (GridRows.Settings, Auto),
                    (GridRows.Input, new UIGridLength(1, UIGridUnitType.Fraction)),
                    (GridRows.Output, new UIGridLength(1, UIGridUnitType.Fraction)))

                .Columns(
                    (GridColumns.Stretch, new UIGridLength(1, UIGridUnitType.Fraction)))

                .Cells(
                    Cell(
                        GridRows.Settings,
                        GridColumns.Stretch,

                        Stack()
                            .Vertical()
                            .SmallSpacing()

                            .WithChildren(
                                Stack()
                                .Vertical()
                                .WithChildren(
                                    Label()
                                        .Text(UrlEncoderDecoder.ConfigurationTitle),
                                    SettingGroup()
                                        //Setting("url-conversion-mode-setting")
                                        .Icon("FluentSystemIcons", '\uF18D')
                                        .Title(UrlEncoderDecoder.ConversionTitle)
                                        .Description(UrlEncoderDecoder.ConversionDescription)

                                        .InteractiveElement(
                                            _conversionModeSwitch
                                                .OnText(UrlEncoderDecoder.ConversionEncode)
                                                .OffText(UrlEncoderDecoder.ConversionDecode)
                                                .OnToggle(OnConversionModeChanged))
                                    .WithSettings(
                                        Setting("url-conversion-multiple-setting")
                                            .Icon("FluentSystemIcons", '\uF18D')
                                            .Title("Encoding multiple")
                                            .Description("Encode or decode multiple times.")
                                            .InteractiveElement(
                                                _multilineModeSwitch
                                                    .OnToggle(OnMultilineModeChanged)
                                            )
                                        )
                                    )
                                )),
                    Cell(
                        GridRows.Input,
                        GridColumns.Stretch,

                        _inputText
                            .Title(UrlEncoderDecoder.InputTitle)
                            .OnTextChanged(OnInputTextChanged)),

                    Cell(
                        GridRows.Output,
                        GridColumns.Stretch,

                        _outputText
                            .Title(UrlEncoderDecoder.OutputTitle)
                            .ReadOnly()
                            .Extendable())));

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _semaphore.Dispose();
    }

    private void OnConversionModeChanged(bool conversionMode)
    {
        _settingsProvider.SetSetting(UrlEncoderDecoderGuiTool.conversionMode, conversionMode ? EncodingConversion.Encode : EncodingConversion.Decode);
        _inputText.Text(_outputText.Text); // This will trigger a conversion.
    }

    private void OnMultilineModeChanged(bool multilineMode)
    {
        _settingsProvider.SetSetting(UrlEncoderDecoderGuiTool.multilineMode, multilineMode);
        StartConvert(_inputText.Text);
    }

    private void OnInputTextChanged(string text)
    {
        StartConvert(text);
    }

    private void StartConvert(string text)
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();

        WorkTask = ConvertAsync(text, _settingsProvider.GetSetting(conversionMode), _cancellationTokenSource.Token);
    }

    private async Task ConvertAsync(string input, EncodingConversion conversionModeSetting, CancellationToken cancellationToken)
    {
        using (await _semaphore.WaitAsync(cancellationToken))
        {
            await TaskSchedulerAwaiter.SwitchOffMainThreadAsync(cancellationToken);

            if (_settingsProvider.GetSetting(multilineMode))
            {
                List<string> resultLines = [];
                List<string> inputLines = [.. input.Split([Environment.NewLine], StringSplitOptions.None)];

                foreach (string line in inputLines)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    resultLines.Add(
                        UrlHelper.EncodeOrDecode(
                            line,
                            conversionModeSetting,
                            _logger,
                            cancellationToken));
                }

                _outputText.Text(string.Join(Environment.NewLine, resultLines));
            }
            else
            {
                _outputText.Text(
                    UrlHelper.EncodeOrDecode(
                        input,
                        conversionModeSetting,
                        _logger,
                        cancellationToken));
            }
        }
    }
}
