using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class RewindMessage : AbstractPositionMessage
{
    private RewindMessage(long id, Vector2I coordinate, Direction direction) : base(id, coordinate, direction)
    {
    }

    public override void Accept(IServerMessageVisitor visitor)
    {
    }

    public static RewindMessage FromPacket(PositionPacket packet)
    {
        return new RewindMessage(packet.Id, new Vector2I(packet.X, packet.Y), (Direction)packet.Direction);
    }
}