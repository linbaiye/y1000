using System.Collections.Generic;
using Godot;
using NLog;

namespace y1000.Source.Control.NpcInteraction;

public partial class InteractionGroups : HBoxContainer
{

    private readonly Button[] _buttonGroups2 = new Button[4];

    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    public override void _Ready()
    {
        for (int i = 1; i <= 4; i++)
        {
            int idx = i - 1;
            _buttonGroups2[idx] = GetNode<Button>("InteractionGroup2/Interaction" + i);
            _buttonGroups2[idx].Pressed += () => OnButtonPressed(_buttonGroups2[idx]);
        }
    }


    private void OnButtonPressed(Button button)
    {
        GD.Print("Pressed " + button.Text);
        //Logger.Debug("Pressed {0}.", button.Text);
    }
    

    public void Display(List<string> textList)
    {
        for (int i = 0; i < textList.Count && i < _buttonGroups2.Length; i++)
        {
            _buttonGroups2[i].Text = textList[i];
        }
    }
}