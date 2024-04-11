using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Source.Networking.Protobuf;
using Godot;
using y1000.Source.Networking;

namespace y1000.code.networking.message
{
    public class LoginMessage  : IServerMessage
    {
        public Vector2I Coordinate { get; init; }
        
        public long Id { get; init; }
        
        public bool Male { get; init; }

        public static LoginMessage FromPacket(LoginPacket loginPacket)
        {
            return new LoginMessage() { Coordinate = new Vector2I(loginPacket.X, loginPacket.Y), Id = loginPacket.Id, Male = true};
        }

        public override string ToString()
        {
            return $"{nameof(Coordinate)}: {Coordinate}";
        }

        public void Accept(IServerMessageHandler handler)
        {
            handler.Handle(this);
        }
    }
}