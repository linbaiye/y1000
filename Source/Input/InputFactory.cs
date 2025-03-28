using Godot;
using y1000.Source.Creature;
using y1000.Source.Entity;

namespace y1000.Source.Input
{
	public static class InputFactory
	{
		private static long _sequence;
		
		public static MouseRightClick CreateMouseMoveInput(Direction direction)
		{
			return new MouseRightClick(_sequence++, direction);
		}

		public static AttackInput CreateAttack(IEntity target)
		{
			return new AttackInput(_sequence++, target);
		}

		public static MouseRightRelease CreateMouseRightRelease()
		{
			return new MouseRightRelease(_sequence++);
		}

		public static RightMousePressedMotion CreateRightMousePressedMotion(Direction direction)
		{
			return new RightMousePressedMotion(_sequence++, direction);
		}
		
		public static KeyboardPredictableInput KeyInput(Key key)
		{
			return new KeyboardPredictableInput(_sequence++, key);
		}
	}
}
