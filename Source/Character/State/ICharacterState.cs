using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State
{
    public interface ICharacterState
    {
        void OnMouseRightClicked(Character character, MouseRightClick rightClick);

        void OnMouseRightReleased(Character character, MouseRightRelease mouseRightRelease);
        
        void OnMousePressedMotion(Character character, RightMousePressedMotion mousePressedMotion);
        
        bool CanHandle(IInput input);

        void OnWrappedPlayerAnimationFinished(Character character) {}

        IPlayerState WrappedState { get; }
    }
}