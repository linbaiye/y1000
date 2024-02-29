using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.Source.Character.State
{
    public readonly struct TimeFrameMapper
    {


        private readonly double _end;

        private readonly int _frameOffset;

        public TimeFrameMapper(double end, int frameOffset)
        {
            _end = end;
            _frameOffset = frameOffset;
        }


        public double Time => _end;


        public int FrameOffset => _frameOffset;
    }
}