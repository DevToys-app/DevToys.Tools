using System.Text;
using System.Text.RegularExpressions;

namespace DevToys.Tools.Tools.Text.LineFilter;

[Export(typeof(IGuiTool))]
[Name("LineFilter")]
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons",
    IconGlyph = '\uF211',
    GroupName = PredefinedCommonToolGroupNames.Text,
    ResourceManagerAssemblyIdentifier = nameof(DevToysToolsResourceManagerAssemblyIdentifier),
    ResourceManagerBaseName = "DevToys.Tools.Tools.Text.LineFilter.LineFilter",
    ShortDisplayTitleResourceName = nameof(LineFilter.ShortDisplayTitle),
    LongDisplayTitleResourceName = nameof(LineFilter.LongDisplayTitle),
    DescriptionResourceName = nameof(LineFilter.Description),
    AccessibleNameResourceName = nameof(LineFilter.AccessibleName),
    SearchKeywordsResourceName = nameof(LineFilter.SearchKeywords))]
internal sealed class LineFilterGuiTool : IGuiTool
{
    private enum MatchMode
    {
        PlainText,
        Keywords,
        Regex
    }

    private enum GridRow
    {
        Header,
        Content
    }

    private enum GridColumn
    {
        Stretch
    }

    private readonly IUIMultiLineTextInput _inputTextArea = MultiLineTextInput("line-filter-input");
    private readonly IUIMultiLineTextInput _outputTextArea = MultiLineTextInput("line-filter-output");
    private readonly IUISingleLineTextInput _patternInput = SingleLineTextInput("line-filter-pattern");
    private readonly IUILabel _summaryLabel = Label("line-filter-summary");
    private readonly IUIInfoBar _errorInfoBar = InfoBar("line-filter-error");

    private MatchMode _mode = MatchMode.PlainText;
    private bool _caseSensitive;
    private bool _invertMatch;

    [ImportingConstructor]
    public LineFilterGuiTool()
    {
        _inputTextArea.OnTextChanged(_ => RefilterAsync());
        _patternInput.OnTextChanged(_ => RefilterAsync());
    }

    public UIToolView View
        => new(
            isScrollable: false,
            Grid()
                .RowMediumSpacing()
                .Rows(
                    (GridRow.Header, UIGridLength.Auto),
                    (GridRow.Content, new UIGridLength(1, UIGridUnitType.Fraction)))
                .Columns(
                    (GridColumn.Stretch, new UIGridLength(1, UIGridUnitType.Fraction)))
                .Cells(
                    Cell(
                        GridRow.Header,
                        GridColumn.Stretch,
                        Stack()
                            .Vertical()
                            .SmallSpacing()
                            .WithChildren(

                                Label()
                                    .Style(UILabelStyle.Subtitle)
                                    .Text(LineFilter.ConfigurationTitle),

                                Stack()
                                    .Horizontal()
                                    .MediumSpacing()
                                    .WithChildren(

                                        SelectDropDownList("line-filter-mode")
                                            .Title(LineFilter.ModeLabel)
                                            .WithItems(
                                                Item(LineFilter.ModePlainText, MatchMode.PlainText),
                                                Item(LineFilter.ModeKeywords, MatchMode.Keywords),
                                                Item(LineFilter.ModeRegex, MatchMode.Regex))
                                            .Select(0)
                                            .OnItemSelected(OnModeSelected),

                                        Switch("line-filter-case-sensitive")
                                            .Off()
                                            .OnText(LineFilter.CaseSensitiveOn)
                                            .OffText(LineFilter.CaseSensitiveOff)
                                            .OnToggle(OnCaseSensitiveToggled),

                                        Switch("line-filter-invert-match")
                                            .Off()
                                            .OnText(LineFilter.InvertMatchOn)
                                            .OffText(LineFilter.InvertMatchOff)
                                            .OnToggle(OnInvertMatchToggled)),

                                _patternInput
                                    .Title(LineFilter.PatternLabel),

                                Label()
                                    .Style(UILabelStyle.Caption)
                                    .Text(LineFilter.PatternDescription),

                                _errorInfoBar
                                    .Error()
                                    .Closable()
                                    .Close(),

                                _summaryLabel.Text(string.Empty))),

                    Cell(
                        GridRow.Content,
                        GridColumn.Stretch,
                        SplitGrid()
                            .Vertical()
                            .TopPaneLength(new UIGridLength(1, UIGridUnitType.Fraction))
                            .BottomPaneLength(new UIGridLength(3, UIGridUnitType.Fraction))
                            .MinimumLength(80)
                            .WithTopPaneChild(
                                _inputTextArea
                                    .Title(LineFilter.InputTextLabel))
                            .WithBottomPaneChild(
                                _outputTextArea
                                    .Title(LineFilter.OutputTextLabel)
                                    .ReadOnly()
                                    .Extendable()
                                    .CanCopyWhenEditable()))));

    internal static readonly char[] separator = new[] { ' ', '\t', '\r', '\n' };

    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        if (parsedData is string stringData)
        {
            _inputTextArea.Text(stringData);
        }
    }

    private ValueTask OnModeSelected(IUIDropDownListItem? item)
    {
        if (item?.Value is MatchMode mode)
        {
            _mode = mode;
        }

        return RefilterAsync();
    }

    private ValueTask OnCaseSensitiveToggled(bool value)
    {
        _caseSensitive = value;
        return RefilterAsync();
    }

    private ValueTask OnInvertMatchToggled(bool value)
    {
        _invertMatch = value;
        return RefilterAsync();
    }

    private ValueTask RefilterAsync()
    {
        try
        {
            string pattern = _patternInput.Text ?? string.Empty;
            string input = _inputTextArea.Text ?? string.Empty;

            if (input.Length == 0)
            {
                _outputTextArea.Text(string.Empty);
                _summaryLabel.Text(string.Empty);
                _inputTextArea.Highlight([]);
                HideError();
                return ValueTask.CompletedTask;
            }

            (int Start, int Length)[] lineRanges = SplitIntoLineRanges(input);

            Func<string, bool>? matcher = BuildMatcher(pattern);
            Func<string, IEnumerable<(int Start, int Length)>>? spanFinder = BuildSpanFinder(pattern);

            if (matcher is null)
            {
                // No pattern => show everything, no highlights.
                _outputTextArea.Text(input);
                _summaryLabel.Text(string.Format(LineFilter.SummaryFormat, lineRanges.Length, lineRanges.Length));
                _inputTextArea.Highlight([]);
                HideError();
                return ValueTask.CompletedTask;
            }

            var output = new StringBuilder(input.Length);
            var highlights = new List<UIHighlightedTextSpan>();
            int matched = 0;

            for (int i = 0; i < lineRanges.Length; i++)
            {
                (int lineStart, int lineLength) = lineRanges[i];
                string line = input.Substring(lineStart, lineLength);

                bool isMatch = matcher(line);

                // Always highlight pattern occurrences in the input, regardless of
                // invert state (invert changes only which lines flow to the output).
                if (isMatch && spanFinder is not null)
                {
                    foreach ((int spanStart, int spanLength) in spanFinder(line))
                    {
                        if (spanLength > 0)
                        {
                            highlights.Add(
                                new UIHighlightedTextSpan(
                                    lineStart + spanStart,
                                    spanLength,
                                    UIHighlightedTextSpanColor.Yellow));
                        }
                    }
                }

                if (_invertMatch)
                {
                    isMatch = !isMatch;
                }

                if (isMatch)
                {
                    if (matched > 0)
                    {
                        output.Append('\n');
                    }

                    output.Append(line);
                    matched++;
                }
            }

            _outputTextArea.Text(output.ToString());
            _summaryLabel.Text(string.Format(LineFilter.SummaryFormat, matched, lineRanges.Length));
            _inputTextArea.Highlight([.. highlights]);
            HideError();
        }
        catch (RegexParseException ex)
        {
            ShowError(LineFilter.InvalidRegexTitle, ex.Message);
            _outputTextArea.Text(string.Empty);
            _summaryLabel.Text(string.Empty);
            _inputTextArea.Highlight([]);
        }
        catch (ArgumentException ex)
        {
            // Older .NET throws ArgumentException for bad regex patterns.
            ShowError(LineFilter.InvalidRegexTitle, ex.Message);
            _outputTextArea.Text(string.Empty);
            _summaryLabel.Text(string.Empty);
            _inputTextArea.Highlight([]);
        }

        return ValueTask.CompletedTask;
    }

    private Func<string, bool>? BuildMatcher(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            return null;
        }

        StringComparison comparison = _caseSensitive
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;

        switch (_mode)
        {
            case MatchMode.PlainText:
                return line => line.Contains(pattern, comparison);

            case MatchMode.Keywords:
                {
                    string[] keywords = pattern
                        .Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (keywords.Length == 0)
                    {
                        return null;
                    }

                    return line =>
                    {
                        for (int i = 0; i < keywords.Length; i++)
                        {
                            if (line.Contains(keywords[i], comparison))
                            {
                                return true;
                            }
                        }

                        return false;
                    };
                }

            case MatchMode.Regex:
                {
                    RegexOptions options = RegexOptions.CultureInvariant;
                    if (!_caseSensitive)
                    {
                        options |= RegexOptions.IgnoreCase;
                    }

                    var regex = new Regex(pattern, options, TimeSpan.FromSeconds(2));
                    return line => regex.IsMatch(line);
                }

            default:
                return null;
        }
    }

    private Func<string, IEnumerable<(int Start, int Length)>>? BuildSpanFinder(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
        {
            return null;
        }

        StringComparison comparison = _caseSensitive
            ? StringComparison.Ordinal
            : StringComparison.OrdinalIgnoreCase;

        switch (_mode)
        {
            case MatchMode.PlainText:
                return line => FindAllOccurrences(line, pattern, comparison);

            case MatchMode.Keywords:
                {
                    string[] keywords = pattern
                        .Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (keywords.Length == 0)
                    {
                        return null;
                    }

                    return line =>
                    {
                        var all = new List<(int, int)>();
                        foreach (string kw in keywords)
                        {
                            all.AddRange(FindAllOccurrences(line, kw, comparison));
                        }

                        return all;
                    };
                }

            case MatchMode.Regex:
                {
                    RegexOptions options = RegexOptions.CultureInvariant;
                    if (!_caseSensitive)
                    {
                        options |= RegexOptions.IgnoreCase;
                    }

                    var regex = new Regex(pattern, options, TimeSpan.FromSeconds(2));
                    return line =>
                    {
                        var all = new List<(int, int)>();
                        foreach (Match m in regex.Matches(line))
                        {
                            if (m.Success && m.Length > 0)
                            {
                                all.Add((m.Index, m.Length));
                            }
                        }

                        return all;
                    };
                }

            default:
                return null;
        }
    }

    private static IEnumerable<(int Start, int Length)> FindAllOccurrences(string line, string needle, StringComparison comparison)
    {
        if (string.IsNullOrEmpty(needle))
        {
            yield break;
        }

        int i = 0;
        while (i <= line.Length - needle.Length)
        {
            int idx = line.IndexOf(needle, i, comparison);
            if (idx < 0)
            {
                yield break;
            }

            yield return (idx, needle.Length);
            i = idx + needle.Length;
        }
    }

    private static (int Start, int Length)[] SplitIntoLineRanges(string text)
    {
        var ranges = new List<(int, int)>();
        int start = 0;
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (c == '\n')
            {
                ranges.Add((start, i - start));
                start = i + 1;
            }
            else if (c == '\r')
            {
                ranges.Add((start, i - start));
                if (i + 1 < text.Length && text[i + 1] == '\n')
                {
                    i++;
                }

                start = i + 1;
            }
        }

        if (start <= text.Length)
        {
            ranges.Add((start, text.Length - start));
        }

        return [.. ranges];
    }

    private void ShowError(string title, string description)
    {
        _errorInfoBar.Title(title).Description(description).Open();
    }

    private void HideError()
    {
        _errorInfoBar.Close();
    }
}
