using System;
using Source.Networking.Protobuf;
using Godot;
using y1000.code.networking.message;

namespace y1000.Source.Input
{
    public abstract class AbstractInput : IPredictableInput
    {
        protected AbstractInput(long s)
        {
            Sequence = s;
        }

        public long Sequence { get; }
        
        public abstract InputType Type { get; }

        public abstract InputPacket ToPacket();
    }
}