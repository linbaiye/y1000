using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.Source.Creature;

namespace y1000.code.entity
{
    public class StateSegment
    {
        public CreatureState State {get; set;}

        public long StateStart {get; set;}

        public long SegmentStart {get; set;}

        public long SegmentDuration { get; set; }
    }
}