using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature;

namespace y1000.Source.Networking.Server;

public class HurtMessage : AbstractEntityMessage
{
    private HurtMessage(long id, Vector2I coordinate, Direction direction, CreatureState afterHurtState, int currentLife, int maxLife) : base(id)
    {
        Coordinate = coordinate;
        Direction = direction;
        AfterHurtState = afterHurtState;
        CurrentLife = currentLife;
        MaxLife = maxLife;
    }
    public Vector2I Coordinate { get; }
    
    public Direction Direction { get; }
    
    public CreatureState AfterHurtState { get; }

    public int LifePercent => (CurrentLife * 100) / MaxLife;
    
    public int CurrentLife { get; }
    
    public int MaxLife { get; }
    
    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public override string ToString()
    {
        return
            $"{nameof(Coordinate)}: {Coordinate}, {nameof(Direction)}: {Direction}, {nameof(AfterHurtState)}: {AfterHurtState}";
    }

    public static HurtMessage FromPacket(CreatureHurtEventPacket packet)
    {
        return new HurtMessage(packet.Id, new(packet.X, packet.Y), (Direction)packet.Direction, (CreatureState)packet.AfterHurtState, 
            packet.CurrentLife, packet.MaxLife);
    }
}