using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.player;

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

        protected virtual int GetTextureWidth()
        {
            return Texture.GetWidth() - 20;
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