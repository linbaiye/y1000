namespace y1000.Source.KungFu.Foot;

public class UnnamedBufa : AbstractLevelKungFu, IFootKungFu
{
    public const string NAME = "无名步法";
    
    public UnnamedBufa(float level) : base(level)
    {
    }

    public override string Name => NAME;
}