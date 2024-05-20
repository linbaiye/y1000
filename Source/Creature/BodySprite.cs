using Godot;
using y1000.Source.Animation;

namespace y1000.Source.Creature
{
	public partial class BodySprite : Sprite2D
	{
		private OffsetTexture OffsetTexture()
		{
			var parent = GetParent<IBody>();
			return parent.BodyOffsetTexture;
		}

		public Vector2 OwnerPosition => GetParent<IBody>().BodyPosition;

		public override void _Process(double delta)
		{
			var texture = OffsetTexture();
			Offset = texture.Offset;
			Texture = texture.Texture;
		}
	}
}
