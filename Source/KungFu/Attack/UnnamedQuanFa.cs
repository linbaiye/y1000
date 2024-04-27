namespace y1000.Source.KungFu.Attack;

public class UnnamedQuanFa : AbstractLevelKungFu
{
    public const string NAME = "无名拳法";
    
    public UnnamedQuanFa(float level) : base(level)
    {
        
    }

    public override string Name => NAME;
}