using Godot;
using y1000.Source.Animation;
using y1000.Source.Creature;

namespace y1000.Source.Entity;

public partial class Teleport : AbstractEntity, IBody
{

    private OffsetTexture _texture;
    
    protected override void MyEvent(InputEvent inputEvent)
    {
    }

    public override OffsetTexture BodyOffsetTexture => _texture;

    public void Init(long id, Vector2I coordinate, string name, OffsetTexture texture)
    {
        base.Init(id, coordinate, name);
        _texture = texture;
        GetNode<Sprite2D>("Body").Material = null;
    }
}