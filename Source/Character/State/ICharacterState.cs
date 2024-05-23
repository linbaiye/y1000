
using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.Player;

namespace y1000.Source.Character.State
{
    public interface ICharacterState
    {
        void OnMouseRightClicked(CharacterImpl character, MouseRightClick rightClick) {}

        void OnMouseRightReleased(CharacterImpl character, MouseRightRelease mouseRightRelease) {}
        
        void OnMousePressedMotion(CharacterImpl character, RightMousePressedMotion mousePressedMotion) {}

        bool CanHandle(IPredictableInput input)
        {
            return false;
        }

        void Attack(CharacterImpl character, AttackInput @event) { }

        void OnWrappedPlayerAnimationFinished(CharacterImpl character) {}

        IPlayerState WrappedState { get; }
    }
}