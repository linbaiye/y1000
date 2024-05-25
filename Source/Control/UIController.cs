using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.Bottom;
using y1000.Source.Control.RightSide;
using y1000.Source.Event;

namespace y1000.Source.Control;

public partial class UIController : CanvasLayer
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    private BottomControl _bottomControl;
    
    private RightControl _rightControl;

    private DropItemUI _dropItemUi;

    public override void _Ready()
    {
        _bottomControl = GetNode<BottomControl>("BottomUI");
        _rightControl = GetNode<RightControl>("RightSideUI");
        _dropItemUi = GetNode<DropItemUI>("DropItemUI");
        BindButtons();
    }

    public void InitEventMediator(EventMediator eventMediator)
    {
        eventMediator.SetComponent(_bottomControl);
        eventMediator.SetComponent(_rightControl);
        _dropItemUi.BindEventMediator(eventMediator);
    }

    private void DisplayMessage(string message)
    {
        _bottomControl.DisplayMessage(new TextEvent(message));
    }

    private void BindButtons()
    {
        _bottomControl.InventoryButton.Pressed += _rightControl.OnInventoryButtonClicked;
    }
    
    public void BindCharacter(CharacterImpl character)
    {
        _bottomControl.BindCharacter(character);
        _rightControl.BindCharacter(character);
    }
}