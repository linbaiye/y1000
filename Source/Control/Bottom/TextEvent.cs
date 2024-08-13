using y1000.Source.Event;

namespace y1000.Source.Control.Bottom;

public class TextEvent : IUiEvent
{
    public TextEvent(string message, ColorType bgColor = ColorType.SAY)
    {
        Message = message;
        ColorType= bgColor;
    }

    public string Message { get; }
    
    public ColorType ColorType { get; }
    
}