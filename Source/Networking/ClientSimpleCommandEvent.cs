using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientSimpleCommandEvent : IClientEvent
{
    public ClientSimpleCommandEvent(SimpleCommand command)
    {
        Command = command;
    }

    public enum SimpleCommand
    {
        NPC_POSITION = 1,
        CLIENT_QUIT = 2,
    }

    public static readonly ClientSimpleCommandEvent NpcPosition = new ClientSimpleCommandEvent(SimpleCommand.NPC_POSITION);
    public static readonly ClientSimpleCommandEvent Quit = new(SimpleCommand.CLIENT_QUIT);

    private SimpleCommand Command { get; }

    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {

            SimpleCommand = new ClientSimpleCommandPacket()
            {
                Command = (int)Command,
            }
        };
    }
}