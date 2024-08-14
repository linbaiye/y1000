using Godot;
using y1000.Source.Creature;
using y1000.Source.Player;

namespace y1000.Source.Character.State;

public class CharacterDraggedState : ICharacterState
{
    private readonly PlayerDraggedState _draggedState;

    public CharacterDraggedState(PlayerDraggedState draggedState)
    {
        _draggedState = draggedState;
    }

    public IPlayerState WrappedState => _draggedState;

    public static CharacterDraggedState Towards(Direction direction)
    {
        return new CharacterDraggedState(new PlayerDraggedState(direction));
    }
}