using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace y1000.Source.Networking.Server
{
    public class UpdateGuildKungFuMessage : IServerMessage
    {
        public enum Command
        {
            OPEN = 1,
            TEXT = 2,
            CLOSE = 3,
        }

        public string Text {get;}

        public UpdateGuildKungFuMessage(int command, string text)
        {
            Text = text;
            _command = (Command)command;
        }

        private Command _command;
        public bool IsClose => _command == Command.CLOSE;

        public bool IsText => _command == Command.TEXT;

        public bool IsOpen => _command == Command.OPEN;

        public void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }


    }
}