using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Code.Networking.Gen;
using y1000.code.networking;

namespace y1000.code.character.state.input
{
    public abstract class AbstractInput : IInput, I2ServerGameMessage
    {
        private readonly long sequence;

        private readonly long t;

        protected AbstractInput(long s)
        {
            sequence = s;
            t = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public long Timestamp => t;

        public long Sequence => sequence;

        public abstract InputType Type { get; }

        public abstract Packet ToPacket();

        public override bool Equals(object? obj)
        {
            return obj is IInput input &&
                   sequence == input.Sequence;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(sequence);
        }

    }
}