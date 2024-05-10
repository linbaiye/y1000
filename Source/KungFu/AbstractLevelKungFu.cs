namespace y1000.Source.KungFu;

public abstract class AbstractLevelKungFu : ILevelKungFu
{
    protected AbstractLevelKungFu(int level)
    {
        Level = level;
    }
    
    public int Level { get; }

    public abstract string Name { get; }
}