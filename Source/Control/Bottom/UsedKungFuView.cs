using Godot;
using y1000.Source.Character;

namespace y1000.Source.Control.Bottom;

public partial class UsedKungFuView : VBoxContainer
{
    private UsedKungFuLabel _label1;
    private UsedKungFuLabel _label2;
    private UsedKungFuLabel _label3;
    private UsedKungFuLabel _label4;

    public override void _Ready()
    {
        _label1 = GetNode<UsedKungFuLabel>("Label1");
        _label2 = GetNode<UsedKungFuLabel>("Label2");
        _label3 = GetNode<UsedKungFuLabel>("Label3");
        _label4 = GetNode<UsedKungFuLabel>("Label4");
    }


    public void BlinkGainExpKungFu(string name)
    {
        UsedKungFuLabel[] list = {_label1, _label2, _label3, _label4};
        foreach (var l in list)
        {
            if (l.BlinkIfMatches(name))
            {
                break;
            }
        }
    }
    

    public void DisplayUsedKungFu(CharacterImpl character)
    {
        UsedKungFuLabel[] list = {_label1, _label2, _label3, _label4};
        foreach (var label in list)
        {
            label.Text = "";
        }
        int index = 0;
        list[index++].SetKungFuName(character.AttackKungFu.Name);
        if (character.ProtectionKungFu != null)
            list[index++].SetKungFuName(character.ProtectionKungFu);
        if (character.AssistantKungFu != null)
            list[index++].SetKungFuName(character.AssistantKungFu);
        if (character.FootMagic != null)
            list[index++].SetKungFuName(character.FootMagic.Name);
        if (character.BreathKungFu != null)
            list[index].SetKungFuName(character.BreathKungFu.Name);
    }
}