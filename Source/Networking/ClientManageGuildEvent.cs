using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;
public class ClientManageGuildEvent : IClientEvent
{
    private readonly ClientPacket _packet;
    private enum Command
    {
        TEACH_KUNGFU = 1,
        INVITE,
        KICK,
        PROMOTE,
    }

    private ClientManageGuildEvent(Command command, string name)
    {
        _packet = new ClientPacket()
        {
            ManageGuild = new ClientManageGuildPacket()
            {
                Target = name,
                Type = (int)command
            }
        };
    }

    public ClientPacket ToPacket()
    {
        return _packet;
    }
    public static ClientManageGuildEvent Invite(string name)
    {
        return new ClientManageGuildEvent(Command.INVITE, name);
    }

    public static ClientManageGuildEvent Teach(string name)
    {
        return new ClientManageGuildEvent(Command.TEACH_KUNGFU, name);
    }
}