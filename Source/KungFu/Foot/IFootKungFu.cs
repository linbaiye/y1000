using System;

namespace y1000.Source.KungFu.Foot;

public interface IFootKungFu : ILevelKungFu
{
    bool CanFly => Level >= 85.01f;
    
    public static IFootKungFu? ByName(string name, float level)
    {
        return name switch
        {
            UnnamedBufa.NAME => new UnnamedBufa(level),
            _ => null,
        };
    }
}