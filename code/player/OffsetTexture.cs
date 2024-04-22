using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    // A texture with offset.
    public class OffsetTexture
    {
        public OffsetTexture(Texture2D texture2D, Vector2 position)
        {
            Texture = texture2D;
            Offset = position;
        }

        public Texture2D Texture { get; }

        public Vector2 Offset { get; private set; }

        public OffsetTexture Add(Vector2 off)
        {
            Offset += off;
            return this;
        }
    }
}