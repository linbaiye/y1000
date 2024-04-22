using Godot;
using y1000.code;
using y1000.code.networking.message;
using y1000.code.player;
using y1000.Source.Map;
using y1000.Source.Player;

namespace y1000.Source.Creature;

public abstract partial class AbstractCreature : Node2D, ICreature, IBody
{
    public string EntityName { get; private set; } = "";

    public long Id { get; protected set; }

    public IMap Map { get; set; } = IMap.Empty;
    
    public Direction Direction { get; set; }

    public Vector2I Coordinate => Position.ToCoordinate();
    
    public abstract OffsetTexture BodyOffsetTexture { get; }
    
    protected void Init(long id, Direction direction, Vector2I coordinate, IMap map)
    {
        Direction = direction;
        Id = id;
        Position = coordinate.ToPosition();
        Map = map;
        map.Occupy(this);
    }

    public void SetPosition(AbstractPositionMessage positionMessage)
    {
        Direction = positionMessage.Direction;
        Position = positionMessage.Coordinate.ToPosition();
        Map.Occupy(this);
    }
}