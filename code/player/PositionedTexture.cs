using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public class PositionedTexture
    {
        private readonly Texture2D texture2D;
        private readonly Vector2 position;

        public PositionedTexture(Texture2D texture2D, Vector2 position)
        {
            this.texture2D = texture2D;
            this.position = position;
        }

        public Texture2D Texture => texture2D;
        public Vector2 Position => position;
    }
}