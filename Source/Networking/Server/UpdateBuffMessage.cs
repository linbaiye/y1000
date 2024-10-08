using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Networking.Server
{
    public class UpdateBuffMessage : IServerMessage
    {
        public enum UpdateType
        {
            GAIN = 1,
            FADE = 2
        }

        public int Seconds {get;}

        public UpdateBuffMessage(int seconds,
         Texture2D? texture2D,
            UpdateType t, string? d)
        {
            Seconds = seconds;
            Description = d;
            _type = t;
            Texture = texture2D;
        }


        public Texture2D? Texture {get;}

        public string? Description { get; }

        private UpdateType _type;

        public bool IsGain => _type == UpdateType.GAIN;

        public bool IsFade => _type == UpdateType.FADE;


        public static UpdateBuffMessage Parse(UpdateBuffPacket packet, IconReader iconReader)
        {
            if (packet.Type == (int)UpdateType.GAIN)
            {
                var texture2D = iconReader.Get(packet.Icon);
                if (texture2D == null)
                    throw new NotSupportedException("unknown icon:" + packet.Icon);
                return new UpdateBuffMessage(packet.Seconds, texture2D, (UpdateType)packet.Type, packet.Description);
            }
            else if (packet.Type == (int)UpdateType.FADE)
            {
                return new UpdateBuffMessage(0, null, (UpdateType)packet.Type, null);
            }
            throw new NotSupportedException("unknown type:" + packet.Type);
        }

        public void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}