using Source.Networking.Protobuf;
using Godot;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class MovementEvent : IClientEvent
{
    private readonly IInput _input;
    public MovementEvent(IInput i, Vector2I happenedAt)
    {
        _input = i;
        HappenedAt = happenedAt;
    }
    
    private Vector2I HappenedAt { get; }

    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            MoveEventPacket = new MoveEventPacket()
            {
                Input = _input.ToPacket(),
                HappenedAtX = HappenedAt.X,
                HappenedAtY = HappenedAt.Y,
            }
        };
    }

    public override string ToString()
    {
        return "Input: (" + _input + "), HappenedAt: " + HappenedAt;
    }
}