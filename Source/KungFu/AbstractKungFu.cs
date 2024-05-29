namespace y1000.Source.KungFu;

public abstract class AbstractKungFu : IKungFu
{

    protected AbstractKungFu(string name, int level)
    {
        Name = name;
        Level = level;
    }

    public int Level { get; }
    public string Name { get; }
}