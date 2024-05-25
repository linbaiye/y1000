using y1000.Source.Event;

namespace y1000.Source.Control.Bottom;

public class TextEvent : IUiEvent
{
    public TextEvent(string message, TextColor? color = null, BgColor? bgColor = null)
    {
        Message = message;
        Color = color;
        BgColor = bgColor;
    }

    public string Message { get; }
    
    public TextColor? Color { get; }
    
    public BgColor? BgColor { get; }
    
}