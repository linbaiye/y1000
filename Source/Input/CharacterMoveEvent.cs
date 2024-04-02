using Code.Networking.Gen;
using Godot;
using y1000.code.networking.message;

namespace y1000.Source.Input;

public class CharacterMoveEvent : I2ServerGameMessage
{
    public CharacterMoveEvent(IInput i, Vector2I happenAt)
    {
        Input = i;
        HappenAt = happenAt;
    }
    
    public IInput Input { get; }
    
    public Vector2I HappenAt { get; }

    public Packet ToPacket()
    {
        throw new System.NotImplementedException();
    }
}