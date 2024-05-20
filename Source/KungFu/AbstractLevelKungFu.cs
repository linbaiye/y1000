using System;

namespace y1000.Source.KungFu;

public abstract class AbstractLevelKungFu : ILevelKungFu
{
    protected static readonly Random RANDOM = new();

    protected AbstractLevelKungFu(int level, string name)
    {
        Level = level;
        Name = name;
    }
    
    public int Level { get; }

    public string Name { get; }
}