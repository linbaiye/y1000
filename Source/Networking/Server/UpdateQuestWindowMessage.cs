using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server
{
    public class UpdateQuestWindowMessage : IServerMessage
    {

        public long Id { get; private set;}

        public string Description {get; private set;} = "";

        public string QuestName {get;private set;} = "";

        public string NpcName {get; private set;} = "";

        public string SubmitText {get; private set;} = "";

        public void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }

        public static UpdateQuestWindowMessage Parse(UpdateQuestWindowPacket packet)
        {
            return new UpdateQuestWindowMessage()
            {
                Id = packet.Id,
                Description = packet.QuestDescription,
                QuestName = packet.QuestName,
                NpcName = packet.NpcName,
                SubmitText = packet.SubmitText,
            };
        }
    }
}