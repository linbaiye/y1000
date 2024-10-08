using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientFoundGuildEvent : IClientEvent
{
    private readonly ClientPacket _packet;

    public ClientFoundGuildEvent(string name, Vector2I coordinate, int slot)
    {
        _packet = new ClientPacket()
        {
            FoundGuild = new ClientFoundGuildPacket()
            {
                Name = name,
                X = coordinate.X,
                Y = coordinate.Y,
                InventorySlot = slot
            }
        };
    }
    
    public ClientPacket ToPacket()
    {
        return _packet;
    }
}