using Godot;

namespace y1000.Source.Animation
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