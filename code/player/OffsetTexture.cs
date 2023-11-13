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
        private readonly Texture2D texture2D;
        private readonly Vector2 offset;

        public OffsetTexture(Texture2D texture2D, Vector2 position)
        {
            this.texture2D = texture2D;
            offset = position;
        }

        public Texture2D Texture => texture2D;
        public Vector2 Offset => offset;
    }
}