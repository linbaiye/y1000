using System.Net.Mime;
using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientTextEvent : IClientEvent
{
    public ClientTextEvent(string text)
    {
        Text = text;
    }

    private string Text { get; }

    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            Say = new ClientSayPacket()
            {
                Text = Text
            }
        };
    }
}