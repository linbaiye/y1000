namespace y1000.Source.KungFu;

public abstract class AbstractLevelKungFu : ILevelKungFu
{
    protected AbstractLevelKungFu(float level)
    {
        Level = level;
    }
    
    public float Level { get; }

    public abstract string Name { get; }
}