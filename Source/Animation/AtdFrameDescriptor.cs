namespace y1000.Source.Animation;

public class AtdFrameDescriptor
{
    public AtdFrameDescriptor(int number, int xOffset = 0, int yOffset = 0)
    {
        Number = number;
        XOffset = xOffset;
        YOffset = yOffset;
    }

    public int Number { get; }
    
    public int XOffset { get; }
    
    public int YOffset { get; }

    public override string ToString()
    {
        return $"{nameof(Number)}: {Number}, {nameof(XOffset)}: {XOffset}, {nameof(YOffset)}: {YOffset}";
    }
}