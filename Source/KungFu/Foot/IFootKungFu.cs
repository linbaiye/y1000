using System;

namespace y1000.Source.KungFu.Foot;

public interface IFootKungFu : ILevelKungFu
{
    bool CanFly => Level >= 8501;
    
    public static IFootKungFu? ByName(string name, int level)
    {
        return name switch
        {
            UnnamedBufa.NAME => new UnnamedBufa(level),
            _ => null,
        };
    }
}