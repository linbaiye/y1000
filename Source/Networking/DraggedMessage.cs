using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class DraggedMessage : AbstractPositionMessage
{
	public DraggedMessage(long id, Vector2I coordinate, Direction direction) : base(id, coordinate, direction, CreatureState.DIE)
	{
	}

	public static DraggedMessage FromPacket(PositionPacket packet)
	{
		return new DraggedMessage(packet.Id, new Vector2I(packet.X, packet.Y), (Direction)packet.Direction);
	}
	public override void Accept(IServerMessageVisitor visitor)
	{
		visitor.Visit(this);
	}

	public override string ToString()
	{
		return FormatLog("Dragged");
	}
}
