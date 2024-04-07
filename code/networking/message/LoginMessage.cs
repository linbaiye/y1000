using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Source.Networking.Protobuf;
using Godot;
using y1000.code.networking.message.character;

namespace y1000.code.networking.message
{
    public class LoginMessage 
    {

        public Vector2I Coordinate { get; init; }

        public static LoginMessage FromPacket(LoginPacket loginPacket)
        {
            return new LoginMessage() { Coordinate = new Vector2I(loginPacket.X, loginPacket.Y)  };
        }

        public override string ToString()
        {
            return $"{nameof(Coordinate)}: {Coordinate}";
        }
    }
}