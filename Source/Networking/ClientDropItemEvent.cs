using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Character.Event;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientDropItemEvent : AbstractInventoryEvent, IClientEvent
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

    public ClientDropItemEvent(int slot, int number, Vector2 position, Vector2I coordinate) : base(slot)
    {
        Number = number;
        Position = position;
        Coordinate = coordinate;
    }
}