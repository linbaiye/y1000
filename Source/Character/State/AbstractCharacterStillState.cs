using y1000.Source.Character.Event;
using y1000.Source.Character.State.Prediction;
using y1000.Source.Input;
using y1000.Source.Player;
using y1000.Source.Util;
using ILogger = NLog.ILogger;

namespace y1000.Source.Character.State;

public abstract class AbstractCharacterStillState : ICharacterState
{
    protected AbstractCharacterStillState(IPlayerState wrappedState)
    {
        WrappedState = wrappedState;
    }
    public IPlayerState WrappedState { get; }
    
    public void OnMouseRightClicked(CharacterImpl character, MouseRightClick rightClick)
    {
        HandleRightClick(character, rightClick);
    }
    
    protected abstract ILogger Logger { get; }

    
    protected abstract ICharacterState MoveState(CharacterImpl character, AbstractRightClickInput rightClickInput);

    private void HandleRightClick(CharacterImpl character, AbstractRightClickInput rightClick)
    {
        if (!character.CanMoveOneUnit(rightClick.Direction))
        {
            if (character.Direction != rightClick.Direction)
            {
                character.EmitPredictionEvent(new SetPositionPrediction(rightClick, character.Coordinate, rightClick.Direction), 
                    new PredictMovementEvent(rightClick, character.Coordinate));
                character.Direction = rightClick.Direction;
                WrappedState.Reset();
            }
        }
        else
        {
            Logger.Debug("Moving to coordinate {0}.", character.Coordinate.Move(rightClick.Direction));
            character.EmitPredictionEvent(new MovePrediction(rightClick, character.Coordinate, rightClick.Direction),
                new PredictMovementEvent(rightClick, character.Coordinate));
            var characterState = character.FootMagic != null ? CharacterMoveState.Move(character.FootMagic, rightClick) :
                MoveState(character, rightClick);
            character.ChangeState(characterState);
        }
    }

    public bool CanHandle(IPredictableInput input)
    {
        return true;
    }

    public bool CanSitDown()
    {
        return true;
    }

    public void OnMousePressedMotion(CharacterImpl character, RightMousePressedMotion mousePressedMotion)
    {
        HandleRightClick(character, mousePressedMotion);
    }


    public bool CanAttack()
    {
        return true;
    }
    
    public void OnWrappedPlayerAnimationFinished(CharacterImpl character)
    {
        WrappedState.Reset();
    }
}