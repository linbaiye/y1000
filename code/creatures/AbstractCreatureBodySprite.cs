
using Godot;
using y1000.Source.Animation;

namespace y1000.code.creatures
{
    public abstract partial class AbstractCreatureBodySprite : Sprite2D, ICreatureBodySprite
    {
        private BodyHoverRect? bodyHoverRect;

        private static readonly Rect2I EMPTY = new Rect2I(0,0,0,0);

        public override void _Ready()
        {
            bodyHoverRect = GetNode<BodyHoverRect>("BodyHoverRect");
        }

        protected abstract OffsetTexture GetPositionedTexture();

        public Rect2I HoverRect()
        {
            return bodyHoverRect != null ?  bodyHoverRect.HoverRect() : EMPTY;
        }

        protected int GetTextureWidth()
        {
            return Texture.GetWidth() - 20;
        }

        public Vector2 GlobalTextureCenter(Vector2 ownerPosition)
        {
            var pos = ownerPosition + Offset;
            return new Vector2(pos.X + GetTextureWidth() / 2, pos.Y + Texture.GetHeight() / 2);
        }

        public override void _Process(double delta)
        {
            var positionedTexture = GetPositionedTexture();
            Offset = positionedTexture.Offset;
            Texture = positionedTexture.Texture;
            bodyHoverRect?.OnTextureChanged(Offset, GetTextureWidth(), Texture.GetHeight());
        }
    }
}