using Godot;
using Source.Networking.Protobuf;
using y1000.code;

namespace y1000.Source.Networking;

public class Interpolation
{
    private Interpolation(Vector2I coordinate, CreatureState state, long elapsedMillis, Direction direction, long id)
    {
        Coordinate = coordinate;
        State = state;
        ElapsedMillis = elapsedMillis;
        Direction = direction;
        Id = id;
    }

    public Vector2I Coordinate { get; }
    
    public CreatureState State { get; }
    
    public long ElapsedMillis { get; }
    
    public Direction Direction { get; }
    
    public long Id { get; }

    public override string ToString()
    {
        return $"{nameof(Coordinate)}: {Coordinate}, {nameof(State)}: {State}, {nameof(ElapsedMillis)}: {ElapsedMillis}, {nameof(Direction)}: {Direction}, {nameof(Id)}: {Id}";
    }

    public static Interpolation FromPacket(InterpolationPacket packet)
    {
        return new Interpolation(new Vector2I(packet.X, packet.Y), (CreatureState)packet.State, packet.ElapsedMillis,
            (Direction)packet.Direction, packet.Id);
    }

}