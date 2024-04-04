using System;
using Code.Networking.Gen;
using Godot;
using y1000.code.networking.message;

namespace y1000.Source.Input
{
    public abstract class AbstractInput : IInput
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