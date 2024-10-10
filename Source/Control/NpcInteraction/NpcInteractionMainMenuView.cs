using System.Collections.Generic;
using Godot;

namespace y1000.Source.Control.NpcInteraction;

public partial class NpcInteractionMainMenuView : NinePatchRect
{
    private InteractionGroups _interactionGroups;

    public override void _Ready()
    {
        _interactionGroups = GetNode<InteractionGroups>("InteractionGroups");
        _interactionGroups.Display(new List<string>() {"买物品", "卖物品", "改良护具", "强化武器"});
    }
    
}