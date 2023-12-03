using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using y1000.code.util;

namespace y1000.code.networking.message
{
    public class MoveMessage : AbstractMovementMessage
    {
        public MoveMessage(int _id, Direction direction, Point coor, long timestamp) : base(_id, direction, coor, timestamp) {}

        public override MovementType MovementType => MovementType.MOVE;
    }
}