using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using y1000.code.networking;
using y1000.code.networking.message;

namespace y1000.code.character.state.snapshot
{
    public class PostionDirectionSnapshot : IStateSnapshot
    {
        public Point Coordinate { get; set; }

        public Direction Direction {get; set;}

        public bool Match(IUpdateStateMessage message)
        {
            return false;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}