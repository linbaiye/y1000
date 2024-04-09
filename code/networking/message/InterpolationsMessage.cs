using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.player;
using y1000.Source.Networking;

namespace y1000.code.networking.message
{
    public class InterpolationsMessage : IEntityMessage
    {
        private readonly List<IInterpolation> _interpolations;

        public InterpolationsMessage(List<IInterpolation> interpolations)
        {
            _interpolations = interpolations;
        }

        public long Id => throw new NotImplementedException();

        public List<IInterpolation> Interpolations => _interpolations;

        public void Accept(IServerMessageHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}