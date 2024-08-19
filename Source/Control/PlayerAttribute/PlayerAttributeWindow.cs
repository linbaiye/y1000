using System.Collections.Generic;
using Godot;

namespace y1000.Source.Control.PlayerAttribute;

public partial class PlayerAttributeWindow : NinePatchRect
{

    private PlayerAttributeGroup _group1;
    
    private PlayerAttributeGroup _group2;

    public override void _Ready()
    {
        _group1 = GetNode<PlayerAttributeGroup>("Group1");
        _group2 = GetNode<PlayerAttributeGroup>("Group2");
        GetNode<Button>("Button").Pressed += () => Visible = false;
        Visible = false;
    }

    public void DisplayAttributes(List<string> attributes)
    {
        if (attributes.Count <= 10)
        {
            return;
        }
        _group1.DisplayAttributes(attributes);
        _group2.DisplayAttributes(attributes.GetRange(10, attributes.Count - 10));
        Visible = true;
    }
    
}