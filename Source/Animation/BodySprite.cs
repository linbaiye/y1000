using Godot;
using y1000.Source.Creature;

namespace y1000.Source.Animation
{
	public partial class BodySprite : AbstractPartSprite
	{
		public Vector2 OwnerPosition => GetParent<IBody>().BodyPosition;

		protected override OffsetTexture OffsetTexture => GetParent<IBody>().BodyOffsetTexture;
		
		public override void _Process(double delta)
		{
			var texture = OffsetTexture;
			Offset = texture.Offset;
			Texture = texture.Texture;
		}
	}
}
