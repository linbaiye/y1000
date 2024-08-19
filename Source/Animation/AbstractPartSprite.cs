using Godot;

namespace y1000.Source.Animation;

public abstract partial class AbstractPartSprite : Sprite2D
{
    protected abstract OffsetTexture? OffsetTexture { get; }

    public override void _Process(double delta)
    {
        var texture = OffsetTexture;
        if (texture != null)
        {
            Offset = texture.Offset;
            Texture = texture.Texture;
        }
    }
}