using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature;

namespace y1000.Source.Networking;

public class Interpolation
{
	public Interpolation(Vector2I coordinate, CreatureState state, int elapsedMillis, Direction direction)
	{
		Coordinate = coordinate;
		State = state;
		ElapsedMillis = elapsedMillis;
		Direction = direction;
	}

	public Vector2I Coordinate { get; }
	
	public CreatureState State { get; }
	
	public int ElapsedMillis { get; }
	
	public Direction Direction { get; }
	
	public override string ToString()
	{
		return $"{nameof(Coordinate)}: {Coordinate}, {nameof(State)}: {State}, {nameof(ElapsedMillis)}: {ElapsedMillis}, {nameof(Direction)}: {Direction}";
	}

	public static Interpolation FromPacket(InterpolationPacket packet)
	{
		return new Interpolation(new Vector2I(packet.X, packet.Y), (CreatureState)packet.State, packet.ElapsedMillis,
			(Direction)packet.Direction);
	}

}
