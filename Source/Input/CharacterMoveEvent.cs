using Code.Networking.Gen;
using Godot;
using y1000.code.networking.message;

namespace y1000.Source.Input;

public class CharacterMoveEvent : IClientEvent
{
    private readonly IInput _input;
    public CharacterMoveEvent(IInput i, Vector2I happenedAt)
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
}