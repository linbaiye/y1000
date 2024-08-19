using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.networking.message.character
{
    public interface ICharacterMessage
    {
        long InputSeqeunce { get; }
    }
}