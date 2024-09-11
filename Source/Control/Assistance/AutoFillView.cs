using Godot;
using y1000.Source.Assistant;
using y1000.Source.Character;
using y1000.Source.Util;

namespace y1000.Source.Control.Assistance;

public partial class AutoFillView : NinePatchRect
{
    
    private HealItemView _engeryView;
    private HealItemView _powerView;
    private HealItemView _innerPowerView;
    private HealItemView _outerPowerView;
    private HealItemView _lifeView;
    private HealItemView _lowLifeView;

    private CheckBox _grindExp;
    private LineEdit _interval;

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
        _grindExp = GetNode<CheckBox>("GrindExp");
        _interval = GetNode<LineEdit>("Interval");
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
        int v = 0;
        if (_interval.Text != null && _interval.Text.DigitOnly())
        {
            v = _interval.Text.ToInt();
        }
        _autoFillAssistant.UpdateExpAndInterval(_grindExp.ButtonPressed, v);
        _autoFillAssistant.OnViewClosed();
    }

    private void SetValues(HealItemView itemView, AutoFillAssistant.Setting setting, CharacterImpl character)
    {
        itemView.BindInventory(character.Inventory);
        itemView.SetValues(setting.Enabled, setting.Threshold, setting.UseItem);
    }

    public void BindCharacter(CharacterImpl character, AutoFillAssistant autoFillAssistant)
    {
        _character = character;
        _autoFillAssistant = autoFillAssistant;
        SetValues(_innerPowerView, autoFillAssistant.GetSettingsOrDefault(AutoFillOption.INNER_POWER), character);
        SetValues(_outerPowerView, autoFillAssistant.GetSettingsOrDefault(AutoFillOption.OUTER_POWER), character);
        SetValues(_powerView, autoFillAssistant.GetSettingsOrDefault(AutoFillOption.POWER), character);
        SetValues(_lifeView, autoFillAssistant.GetSettingsOrDefault(AutoFillOption.LIFE), character);
        SetValues(_lowLifeView, autoFillAssistant.GetSettingsOrDefault(AutoFillOption.LOW_LIFE), character);
        if (autoFillAssistant.GrindExp)
            _grindExp.ButtonPressed = true;
        _interval.Text = autoFillAssistant.Interval.ToString();
    }
}