using Godot;
using Source.Networking.Protobuf;
using y1000.code;
using y1000.code.networking.message;
using y1000.Source.Creature;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class RunMessage : AbstractPositionMessage
{
    public RunMessage(long id, Vector2I coordinate, Direction direction) : base(id, coordinate, direction)
    {
    }

    public static RunMessage FromPacket(PositionPacket packet)
    {
        return new RunMessage(packet.Id, new Vector2I(packet.X, packet.Y), (Direction)packet.Direction);
    }
    public override void HandleBy(IServerMessageHandler handler)
    {
        handler.Handle(this);
    }

    public override string ToString()
    {
        return FormatLog("Run");
    }
}