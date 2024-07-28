namespace y1000.Source.Networking.Server;

public class LifebarMessage : AbstractEntityMessage
{
    public LifebarMessage(long id, int percent) : base(id)
    {
        Percent = percent;
    }

    public int Percent { get; }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}