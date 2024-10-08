using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking
{
    public class ClientSubmitQuestEvent : IClientEvent
    {

        private readonly ClientPacket _clientPacket;

        public ClientSubmitQuestEvent(long id, string name) 
        {
            _clientPacket = new ClientPacket()
            {
                SubmitQuest = new ClientSubmitQuestPacket()
                {
                    Id = id,
                    QuestName = name,
                }
            };
        }


        public ClientPacket ToPacket()
        {
            return _clientPacket;
        }
    }
}