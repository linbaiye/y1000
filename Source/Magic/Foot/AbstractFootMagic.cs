namespace y1000.Source.Magic.Foot;

public abstract class AbstractFootMagic : IFootMagic
{
    protected AbstractFootMagic(float level)
    {
        Level = level;
    }

    public float Level { get; }
}