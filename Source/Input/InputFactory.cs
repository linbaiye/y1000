using Godot;
using y1000.code;
using y1000.Source.Character.Event;
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

        public static AttackEntityInput CreateAttack(IEntity target)
        {
            return new AttackEntityInput(_sequence++, target);
        }

        public static MouseRightRelease CreateMouseRightRelease()
        {
            return new MouseRightRelease(_sequence++);
        }

        public static RightMousePressedMotion CreateRightMousePressedMotion(Direction direction)
        {
            return new RightMousePressedMotion(_sequence++, direction);
        }
        
        public static KeyboardInput KeyInput(Key key)
        {
            return new KeyboardInput(_sequence++, key);
        }
    }
}