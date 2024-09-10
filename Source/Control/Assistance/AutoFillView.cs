using Godot;
using y1000.Source.Assistant;
using y1000.Source.Character;

namespace y1000.Source.Control.Assistance;

public partial class AutoFillView : NinePatchRect
{
    
    private HealItemView _engeryView;
    private HealItemView _powerView;
    private HealItemView _innerPowerView;
    private HealItemView _outerPowerView;
    private HealItemView _lifeView;
    private HealItemView _lowLifeView;

    private CharacterImpl? _character;
    
    private AutoFillAssistant? _autoFillAssistant;

    public override void _Ready()
    {
        _engeryView = GetNode<HealItemView>("Item1");
        _innerPowerView = GetNode<HealItemView>("Item2");
        _outerPowerView = GetNode<HealItemView>("Item3");
        _powerView = GetNode<HealItemView>("Item4");
        _lifeView = GetNode<HealItemView>("Item5");
        _lowLifeView = GetNode<HealItemView>("Item6");
        _engeryView.Disable();
    }

    public void OnClosed()
    {
        if (_autoFillAssistant == null)
        {
            return;
        }
        _autoFillAssistant.UpdateAutoFillItem(AutoFillOption.LIFE, _lifeView);
        _autoFillAssistant.UpdateAutoFillItem(AutoFillOption.POWER, _powerView);
        _autoFillAssistant.UpdateAutoFillItem(AutoFillOption.OUTER_POWER, _outerPowerView);
        _autoFillAssistant.UpdateAutoFillItem(AutoFillOption.INNER_POWER, _innerPowerView);
        _autoFillAssistant.UpdateAutoFillItem(AutoFillOption.LOW_LIFE, _lowLifeView);
    }

    public void BindCharacter(CharacterImpl character, AutoFillAssistant autoFillAssistant)
    {
        _character = character;
        _innerPowerView.BindInventory(character.Inventory);
        _outerPowerView.BindInventory(character.Inventory);
        _powerView.BindInventory(character.Inventory);
        _lifeView.BindInventory(character.Inventory);
        _lowLifeView.BindInventory(character.Inventory);
        _autoFillAssistant = autoFillAssistant;
    }
}