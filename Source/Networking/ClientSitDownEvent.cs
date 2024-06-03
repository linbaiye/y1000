using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientSitDownEvent : IClientEvent
{
    public ClientSitDownEvent(Vector2I coordinate)
    {
        Coordinate = coordinate;
    }

    private Vector2I Coordinate { get; }
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            SitDown = new ClientSitDownPacket()
            {
                X = Coordinate.X,
                Y = Coordinate.Y,
            }
        };
    }
}