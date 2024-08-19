using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.code.networking.message
{
    public class ShowPlayerMessage : IEntityMessage
    {

        private readonly UpdateMovmentStateMessage movmentStateMessage;

        public ShowPlayerMessage(UpdateMovmentStateMessage movmentStateMessage)
        {
            this.movmentStateMessage = movmentStateMessage;
        }

        public UpdateMovmentStateMessage MovmentStateMessage => movmentStateMessage;

        public long Id => movmentStateMessage.Id;
    }
}