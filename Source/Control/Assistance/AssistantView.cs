using Godot;
using y1000.Source.Assistant;
using y1000.Source.Character;

namespace y1000.Source.Control.Assistance;

public partial class AssistantView : NinePatchRect
{

    private Button _close;
    
    private AutoFillView _autoFillView;

    private TextureButton _heal;
    
    private TextureButton _loot;
    

    public override void _Ready()
    {
        Visible = false;
        _close = GetNode<Button>("Close");
        _close.Pressed += OnClosed;
        _autoFillView = GetNode<AutoFillView>("AutoFill");
        _heal = GetNode<TextureButton>("Heal");
        _loot = GetNode<TextureButton>("Loot");
        _heal.Pressed += DisplayHealSettings;
    }

    private void DisplayHealSettings()
    {
        _autoFillView.Visible = true;
    }

    public void OnAssistantButtonClicked()
    {
        _heal.GrabFocus();
        DisplayHealSettings();
        Visible = !Visible;
    }

    public void OnClosed()
    {
        Visible = false;
        _autoFillView.OnClosed();
    }

    public void BindCharacter(CharacterImpl character, AutoFillAssistant autoFillAssistant)
    {
        _autoFillView.BindCharacter(character, autoFillAssistant);
    }
}