using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class LoginEvent : IClientEvent
{
    public LoginEvent(string token, string charName)
    {
        Token = token;
        CharName = charName;
    }

    private string Token { get; set; }
    private string CharName { get; set; }
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            LoginPacket = new PlayerLoginPacket()
            {
                Token = Token,
                CharName = CharName,
            }
        };
    }
}