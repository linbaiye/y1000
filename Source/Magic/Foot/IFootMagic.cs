using System;

namespace y1000.Source.Magic.Foot;

public interface IFootMagic
{
    float Level { get; }
    
    bool CanFly => Level >= 85.01f;
    

    public static IFootMagic ByName(string name, float level)
    {
        return name switch
        {
            UnnamedFootMagic.Name => new UnnamedFootMagic(level),
            _ => throw new NotImplementedException()
        };
    }
}