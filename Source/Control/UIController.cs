using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.Bottom;
using y1000.Source.Control.RightSide;

namespace y1000.Source.Control;

public partial class UIController : CanvasLayer
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    private BottomControl? _bottomControl;
    
    private RightControl? _rightControl;
    
    public override void _Ready()
    {
        _bottomControl = GetNode<BottomControl>("BottomUI");
        _rightControl = GetNode<RightControl>("RightSideUI");
        BindButtons();
    }


    

    private void BindButtons()
    {
        if (_bottomControl == null || _rightControl == null)
        {
            return;
        }
        _bottomControl.InventoryButton.Pressed += _rightControl.OnInventoryButtonClicked;
    }
    
    public void BindCharacter(CharacterImpl character)
    {
        _bottomControl?.BindCharacter(character);
        _rightControl?.BindCharacter(character);
    }
}