using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using Godot;
using y1000.code.networking.message.character;

namespace y1000.code.networking.message
{
    public class LoginMessage 
    {
        public long Id { get; set; }

        public Vector2I Coordinate { get; set; }

        public static LoginMessage FromPacket(LoginPacket loginPacket)
        {
            return new LoginMessage() { Coordinate = new Vector2I(loginPacket.X, loginPacket.Y), Id = loginPacket.Id };
        }
    }
}