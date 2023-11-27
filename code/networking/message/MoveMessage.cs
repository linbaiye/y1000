using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using y1000.code.util;

namespace y1000.code.networking.message
{
    public class MoveMessage : AbstractMovementMessage
    {
        public MoveMessage(int _id) : base(_id)
        {
        }

        public override Packet ToPacket()
        {
            return new Packet()
            {
                MovementPacket = new MovementPacket()
                {
                    X = Coordinate.X,
                    Y = Coordinate.Y,
                    Type = (int)MessageType.MOVE,
                    Direction = (int)Direction,
                    Id = Id(),
                    Timestamp = Timestamp(),
                },
            };
        }
    }
}