using Godot;
using y1000.Source.Character;

namespace y1000.Source.Control.Bottom;

public partial class UsedKungFuView : VBoxContainer
{
    private Label _label1;
    private Label _label2;
    private Label _label3;
    private Label _label4;

    public override void _Ready()
    {
        _label1 = GetNode<Label>("Label1");
        _label2 = GetNode<Label>("Label2");
        _label3 = GetNode<Label>("Label3");
        _label4 = GetNode<Label>("Label4");
    }
    

    public void BindCharacter(CharacterImpl character)
    {
        Label[] list = {_label1, _label2, _label3, _label4};
        foreach (var label in list)
        {
            label.Text = "";
        }
        int index = 0;
        list[index++].Text = character.AttackKungFu.Name;
        if (character.ProtectionKungFu != null)
            list[index++].Text = character.ProtectionKungFu;
        if (character.AssistantKungFu != null)
            list[index++].Text = character.AssistantKungFu;
        if (character.FootMagic != null)
            list[index++].Text = character.FootMagic.Name;
        if (character.BreathKungFu != null)
            list[index].Text = character.BreathKungFu.Name;
    }
}