using Godot;
using y1000.code;

namespace y1000.Source.Input
{
    public static class InputFactory
    {
        private static long _sequence = 0;

        public static MouseRightClick CreateMouseMoveInput(Direction direction)
        {
            return new MouseRightClick(_sequence++, direction);
        }

        public static MouseRightRelease CreateMouseRightRelease()
        {
            return new MouseRightRelease(_sequence++);
        }


        public static RightMousePressedMotion CreateRightMousePressedMotion(Direction direction)
        {
            return new RightMousePressedMotion(_sequence++, direction);
        }
    }
}