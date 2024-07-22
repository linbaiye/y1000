using Godot;
using y1000.Source.Animation;
using y1000.Source.Entity;
using y1000.Source.Map;

namespace y1000.Source.DynamicObject;

public partial class GameDynamicObject : AbstractEntity
{
    public IMap Map { get; private set; } = IMap.Empty;

    protected override void MyEvent(InputEvent inputEvent)
    {
    }
    
    protected void Delete()
    {
        Map.Free(this);
        QueueFree();
    }
    
    protected void Init(long id, Vector2I coordinate, IMap map, string name)
    {
        Init(id, coordinate, name);
        Map = map;
        map.Occupy(this);
    }

    public override OffsetTexture BodyOffsetTexture { get; }
}