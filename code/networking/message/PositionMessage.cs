using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;

namespace y1000.code.networking.message
{
    public class PositionMessage : AbstractMovementMessage
    {
        public PositionMessage(int _id, Direction _dir, Point _point, long timestamp) : base(_id, _dir, _point, timestamp)
        {
        }

        public override MovementType MovementType => MovementType.POSITION;
    }
}