using System;
using System.Collections.Generic;
using Godot;

namespace y1000.Source.Control.PlayerAttribute;

public partial class PlayerAttributeGroup : VBoxContainer
{

    private RichTextLabel[] _attributes = new RichTextLabel[10]; 

    public override void _Ready()
    {
        for (int i = 1; i <= 10; i++)
        {
            _attributes[i - 1] = GetNode<RichTextLabel>("Attribute" + i);
        }
    }

    public void DisplayAttributes(List<string> attriubtes)
    {
        for (int i = 0; i < _attributes.Length; i++)
        {
            _attributes[i].Text = "";
        }
        for (int i = 0; i < _attributes.Length && i < attriubtes.Count; i++)
        {
            _attributes[i].Text = "[color='e7ccb7']" + attriubtes[i] + "[/color]";
        }
    }
}