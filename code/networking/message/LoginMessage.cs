using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;

namespace y1000.code.networking.message
{
    public class LoginMessage : IGameMessage
    {
        public int Id => 0;

        public Point Coordinate {get;set;}

        public Direction Direction => Direction.DOWN;

        public long Timestamp => 0;

        public static LoginMessage FromPacket(LoginPacket loginPacket)
        {
            return new LoginMessage() {Coordinate = new Point(loginPacket.X, loginPacket.Y)};
        }

    }
}