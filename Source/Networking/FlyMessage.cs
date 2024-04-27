using Godot;
using Source.Networking.Protobuf;
using y1000.code;
using y1000.code.networking.message;
using y1000.Source.Creature;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class FlyMessage : AbstractPositionMessage
{
    public FlyMessage(long id, Vector2I coordinate, Direction direction) : base(id, coordinate, direction)
    {
    }

    public override void HandleBy(IServerMessageHandler handler)
    {
        handler.Handle(this);
    }
    
    public static FlyMessage FromPacket(PositionPacket packet)
    {
        return new FlyMessage(packet.Id, new Vector2I(packet.X, packet.Y), (Direction)packet.Direction);
    }
    
    public override string ToString()
    {
        return FormatLog("Fly");
    }
}