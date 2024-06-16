namespace y1000.Source.Networking.Server;

public class CreatureDieMessage : AbstractEntityMessage
{
    public CreatureDieMessage(long id, string sound) : base(id)
    {
        Sound = sound;
    }
    
    public string Sound { get; }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}