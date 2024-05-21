using Godot;
using NLog;
using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.KungFu.Foot;
using y1000.Source.Player;

namespace y1000.Source.Character.State
{
    public class CharacterMoveState : ICharacterState
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        
        private readonly AbstractRightClickInput _trigger;

        private IPredictableInput? _current;
            
        private CharacterMoveState(IPlayerState wrappedState, AbstractRightClickInput trigger)
        {
            WrappedState = wrappedState;
            _trigger = trigger;
        }

        public static CharacterMoveState Move(IFootKungFu? magic, AbstractRightClickInput rightClickInput)
        {
            if (magic != null)
            {
                return magic.CanFly ? Fly(rightClickInput) : Run(rightClickInput);
            }
            return Walk(rightClickInput);
        }


        public void OnWrappedPlayerAnimationFinished(CharacterImpl character)
        {
            character.EmitMoveEvent();
            if (WrappedState.State == CreatureState.ENFIGHT_WALK)
            {
                character.ChangeState(CharacterCooldownState.Cooldown());
            }
            else
            {
                character.ChangeState(CharacterIdleState.Idle());
            }
            if (_current == null && Godot.Input.IsMouseButtonPressed(MouseButton.Right))
            {
                _current = InputFactory.CreateRightMousePressedMotion(_trigger.Direction);
            }
            if (_current != null)
            {
                character.HandleInput(_current);
            }
        }

        public bool CanHandle(IPredictableInput input)
        {
            return input is AbstractRightClickInput or MouseRightRelease;
        }

        public void OnMouseRightClicked(CharacterImpl character, MouseRightClick rightClick)
        {
            _current = rightClick;
        }

        public void OnMousePressedMotion(CharacterImpl character, RightMousePressedMotion mousePressedMotion)
        {
            _current = mousePressedMotion;
        }

        public void OnMouseRightReleased(CharacterImpl character, MouseRightRelease mouseRightRelease)
        {
            _current = mouseRightRelease;
        }

        private static CharacterMoveState Walk(AbstractRightClickInput input)
        {
            return new CharacterMoveState(PlayerMoveState.WalkTowards(input.Direction), input);
        }

        private static CharacterMoveState Run(AbstractRightClickInput input)
        {
            return new CharacterMoveState(PlayerMoveState.RunTowards(input.Direction), input);
        }

        private static CharacterMoveState Fly(AbstractRightClickInput input)
        {
            return new CharacterMoveState(PlayerMoveState.FlyTowards(input.Direction), input);
        }

        public static CharacterMoveState EnfightWalk(AbstractRightClickInput input)
        {
            return new CharacterMoveState(PlayerMoveState.EnfightWalk(input.Direction), input);
        }

        public IPlayerState WrappedState { get; }
    }
}