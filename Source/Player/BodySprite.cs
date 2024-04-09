using Godot;
using y1000.code.player;

namespace y1000.Source.Player
{
	public partial class BodySprite : Sprite2D
	{

		private OffsetTexture OffsetTexture()
		{
			var parent = GetParent<IBody>();
			var texture = parent.BodyOffsetTexture;
			return new OffsetTexture(texture.Texture, texture.Offset + new Vector2(16, -12));
		}

		public override void _Process(double delta)
		{
			var texture = OffsetTexture();
			Offset = texture.Offset;
			Texture = texture.Texture;
		}
	}
}
