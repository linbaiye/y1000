using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Input;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class DropItemMessage : AbstractInventoryEvent, IClientEvent, IServerMessage, ICharacterMessage
{
    private int Number { get; }
    
    private Vector2 Position { get; }
    
    private Vector2I Coordinate { get; }
    
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            DropItem = new DropItemPacket()
            {
                Number = Number,
                X = (int)Position.X,
                Y = (int)Position.Y,
                Slot = Slot,
                CoordinateX = Coordinate.X,
                CoordinateY = Coordinate.Y,
            }
        };
    }

    public DropItemMessage(int slot, int number, Vector2 position, Vector2I coordinate) : base(slot)
    {
        Number = number;
        Position = position;
        Coordinate = coordinate;
    }

    public void Accept(IServerMessageVisitor visitor)
    {
    }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}