using Godot;

namespace y1000.Source.Networking.Server;

public abstract class AbstractNpcInteractionMessage : AbstractUiMessage
{
    protected AbstractNpcInteractionMessage(long id, string name, Texture2D avatar, string text)
    {
        Id = id;
        Name = name;
        Avatar = avatar;
        Text = text;
    }

    public long Id { get; }
    
    public string Name { get; }
    
    public Texture2D Avatar { get; }
    
    public string Text { get; }
    
}