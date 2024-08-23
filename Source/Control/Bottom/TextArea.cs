using System.Collections.Generic;
using System.Text;
using Godot;
using NLog;

namespace y1000.Source.Control.Bottom;

public partial class TextArea : VBoxContainer
{
    private const int MaxSize = 5;
    private readonly Queue<TextEvent> _messages = new(MaxSize);
    private static readonly ILogger LOG = LogManager.GetCurrentClassLogger();

    private RichTextLabel[] _lines = new RichTextLabel[MaxSize];
    private const string ThemeName = "normal";
    
    public override void _Ready()
    {
        for (int i = 0; i < _lines.Length; i++)
            _lines[i] = GetNode<RichTextLabel>("Line" + (i + 1));
    }

    private readonly IDictionary<ColorType, string> _textColors = new Dictionary<ColorType, string>()
    {
        { ColorType.FIRST_GRADE, "#928573" },
        { ColorType.SECOND_GRADE, "#d0bea8" },
        { ColorType.THIRD_GRADE, "#fcf9f3" },
        { ColorType.FOURTH_GRADE, "#b19241" },
        { ColorType.FIVE_GRADE, "#ba6c23" },
        { ColorType.SIX_GRADE, "#fceeaf" },
        { ColorType.SEVEN_GRADE, "#adff2f" },
        { ColorType.EIGHT_GRADE, "#87cefa" },
        { ColorType.NINE_GRADE, "#d8bfd8" },
        { ColorType.TEN_GRADE, "#ffff00" },
        { ColorType.PRIVATE_CHAT, "#e139b2" },
    };

    private readonly IDictionary<ColorType, Color> _bgColors = new Dictionary<ColorType, Color>()
    {
        { ColorType.FIRST_GRADE, new Color("151e2f") },
        { ColorType.SECOND_GRADE, new Color("151e2f") },
        { ColorType.THIRD_GRADE, new Color("151e2f") },
        { ColorType.FOURTH_GRADE, new Color("0d2758") },
        { ColorType.FIVE_GRADE, new Color("0d2758") },
        { ColorType.SIX_GRADE, new Color("0d2758") },
        { ColorType.SEVEN_GRADE, new Color("296d12") },
        { ColorType.EIGHT_GRADE, new Color("004080") },
        { ColorType.NINE_GRADE, new Color("553872") },
        { ColorType.TEN_GRADE, new Color("999900") },
    };

    private StyleBoxFlat? CreateStyleBox(ColorType type)
    {
        if (_bgColors.TryGetValue(type, out var color))
        {
            return new StyleBoxFlat()
            {

                BgColor = color,
            };
        }
        return null;
    }

    private int FindEmptyLine()
    {
        for (int i = 0; i < _lines.Length; i++)
        {
            if (string.IsNullOrEmpty(_lines[i].Text))
            {
                return i;
            }
        }
        return -1;
    }

    private void MakeSpace()
    {
        for (int i = 1; i < _lines.Length; i++)
        {
            _lines[i - 1].Text = _lines[i].Text;
            _lines[i - 1].RemoveThemeStyleboxOverride(ThemeName);
            var themeStylebox = _lines[i].GetThemeStylebox(ThemeName);
            if (themeStylebox != null)
                _lines[i - 1].AddThemeStyleboxOverride(ThemeName, themeStylebox);
        }

        _lines[MaxSize - 1].Text = "";
        _lines[MaxSize - 1].RemoveThemeStyleboxOverride(ThemeName);
    }

    private string CreateText(TextEvent textEvent)
    {
        if (_textColors.TryGetValue(textEvent.ColorType, out var color))
        {
            return "[color=" + color + "]" + textEvent.Message + "[/color]";
        }

        return textEvent.Message;
    }

    public void Display(TextEvent textEvent)
    {
        var line = FindEmptyLine();
        if (line != -1)
        {
            _lines[line].Text = CreateText(textEvent);
            var styleBoxFlat = CreateStyleBox(textEvent.ColorType);
            if (styleBoxFlat != null)
                _lines[line].AddThemeStyleboxOverride(ThemeName, styleBoxFlat);
        }
        else
        {
            MakeSpace();
            Display(textEvent);
        }
    }
}