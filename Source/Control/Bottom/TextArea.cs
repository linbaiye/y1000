using System.Collections.Generic;
using System.Text;
using Godot;
using NLog;

namespace y1000.Source.Control.Bottom;

public partial class TextArea : RichTextLabel
{
    private const int MaxSize = 4;
    private readonly Queue<TextEvent> _messages = new(MaxSize);
    private static readonly ILogger LOG = LogManager.GetCurrentClassLogger();
    public void Display(TextEvent textEvent)
    {
        if (_messages.Count >= 4)
        {
            _messages.Dequeue();
        }
        _messages.Enqueue(textEvent);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var message in _messages)
        {
            stringBuilder.Append(message.Message);
            stringBuilder.Append("\n");
        }
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        Text = stringBuilder.ToString();
    }
}