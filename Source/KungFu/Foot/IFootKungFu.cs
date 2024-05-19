using System;

namespace y1000.Source.KungFu.Foot;

public interface IFootKungFu : ILevelKungFu
{
    bool CanFly => Level >= 8501;

    public static IFootKungFu ByName(string name, int level)
    {
        if (name.Equals("无名步法"))
            return new Bufa(level, name);
        throw new NotImplementedException();
    }
}