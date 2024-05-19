using Source.Networking.Protobuf;
using Godot;
using y1000.Source.Input;

namespace y1000.Source.Character.Event;

public class PredictMovementEvent : IClientEvent
{
    private readonly IRightClickInput _input;
    public PredictMovementEvent(IRightClickInput i, Vector2I happenedAt)
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
                Input = _input.ToRightClickPacket(),
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