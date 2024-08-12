namespace y1000.Source.Networking.Server;

public class EntitySoundMessage : AbstractEntityMessage
{
    public EntitySoundMessage(long id, string sound) : base(id)
    {
        Sound = sound;
    }
    
    public string Sound { get; }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}