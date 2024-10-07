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

        CANCEL_BUFF= 3,
        PING = 4,
        PONG = 5,
    }

    public static readonly ClientSimpleCommandEvent NpcPosition = new ClientSimpleCommandEvent(SimpleCommand.NPC_POSITION);
    public static readonly ClientSimpleCommandEvent Quit = new(SimpleCommand.CLIENT_QUIT);
    public static readonly ClientSimpleCommandEvent CancelBuff = new(SimpleCommand.CANCEL_BUFF);
    public static readonly ClientSimpleCommandEvent PING = new(SimpleCommand.PING);

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