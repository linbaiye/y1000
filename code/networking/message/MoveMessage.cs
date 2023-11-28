using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using y1000.code.util;

namespace y1000.code.networking.message
{
    public class MoveMessage : AbstractMovementMessage, I2ServerGameMessage
    {
        public MoveMessage(int _id) : base(_id)
        {
        }

        public MoveMessage(Direction direction, Point coor, int _id) : base(_id) {
            Direction = direction;
            Coordinate = coor;
        }

        public Packet ToPacket()
        {
            return new Packet()
            {
                MovementPacket = new MovementPacket()
                {
                    X = Coordinate.X,
                    Y = Coordinate.Y,
                    Type = (int)MovementType.MOVE,
                    Direction = (int)Direction,
                    Id = Id,
                    Timestamp = Timestamp,
                },
            };
        }
    }
}