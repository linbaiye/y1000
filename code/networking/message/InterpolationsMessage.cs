using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.player;

namespace y1000.code.networking.message
{
    public class InterpolationsMessage : IGameMessage
    {
        private readonly List<IInterpolation> _interpolations;

        public InterpolationsMessage(List<IInterpolation> interpolations)
        {
            _interpolations = interpolations;
        }

        public int Id => throw new NotImplementedException();

        public long Timestamp => throw new NotImplementedException();

        public List<IInterpolation> Interpolations => _interpolations;
    }
}