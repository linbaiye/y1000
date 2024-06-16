
namespace y1000.Source.KungFu;

public abstract class AbstractLevelKungFu : IKungFu
{
    protected AbstractLevelKungFu(int level, string name)
    {
        Level = level;
        Name = name;
    }
    
    public int Level { get; set; }

    public string Name { get; }

}