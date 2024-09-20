using Godot;
using y1000.Source.Animation;
using y1000.Source.Util;

namespace y1000.Source.Entity;

public abstract partial class AbstractEntity : Node2D, IEntity
{
    private BodySprite? _bodySprite;
    
    public override void _Ready()
    {
        _bodySprite = GetNode<BodySprite>("Body");
        _bodySprite.Area.GuiInput += MyEvent;
        _bodySprite.SetName(EntityName);
    }

    public string EntityName { get; private set; } = "";

    public long Id { get; private set; }

    protected abstract void MyEvent(InputEvent inputEvent);

    public Vector2I Coordinate => Position.ToCoordinate();
    
    public abstract OffsetTexture BodyOffsetTexture { get; }
    
    public Vector2 OffsetBodyPosition => Position + BodyOffsetTexture.Offset;
    
    protected void SetNameColor(int color)
    {
        _bodySprite?.SetNameColor(color);
    }

    protected void Init(long id, Vector2I coordinate, string name)
    {
        Id = id;
        EntityName = name;
        Position = coordinate.ToPosition();
        _bodySprite?.SetName(name);
    }
}