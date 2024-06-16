
using System;
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

        bool CanSitDown()
        {
            return false;
        }

        bool CanStandUp()
        {
            return false;
        }

        bool CanAttack()
        {
            return false;
        }

        void OnWrappedPlayerAnimationFinished(CharacterImpl character) {}

        IPlayerState WrappedState { get; }

        public static ICharacterState Create(CreatureState state)
        {
            switch (state)
            {
                case CreatureState.SIT:
                    return CharacterSitDownState.SitDown();
                case CreatureState.IDLE:
                    return CharacterIdleState.Idle();
                case CreatureState.COOLDOWN:
                    return CharacterCooldownState.Cooldown();
                case CreatureState.DIE:
                    return CharacterDieState.Die();
            }
            throw new NotImplementedException();
        }
    }
}