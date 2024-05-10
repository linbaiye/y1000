using Godot;
using y1000.code.player;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Entity.Animation;

namespace y1000.Source.Player
{
	public partial class BodySprite : Sprite2D
	{
		private OffsetTexture OffsetTexture()
		{
			var parent = GetParent<IBody>();
			return parent.BodyOffsetTexture;
		}

		public Vector2 Coordinate => GetParent<IBody>().Position;

		public override void _Process(double delta)
		{
			var texture = OffsetTexture();
			Offset = texture.Offset;
			Texture = texture.Texture;
		}
	}
}
