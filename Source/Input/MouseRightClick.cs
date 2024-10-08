using System.Numerics;
using Godot;
using y1000.Source.Creature;

namespace y1000.Source.Input
{
	public class MouseRightClick : AbstractRightClickInput
	{
		public MouseRightClick(long s, Direction d) : base(s, d)
		{
		}

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return ((MouseRightClick)obj).Sequence == Sequence;
		}

		public override int GetHashCode()
		{
			return Sequence.GetHashCode();
		}

		public override InputType Type => InputType.MOUSE_RIGHT_CLICK;
	}
}
