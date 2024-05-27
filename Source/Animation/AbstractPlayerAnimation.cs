namespace y1000.Source.Animation;

public abstract class AbstractPlayerAnimation<TA> : AbstractCreatureAnimation<TA> where TA : AbstractCreatureAnimation<TA>
{
    protected static readonly AtdReader AtdReader = AtdReader.LoadPlayer("0.atd");
}