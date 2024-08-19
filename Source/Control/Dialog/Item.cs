using System;
using Godot;
using y1000.Source.Control.RightSide;
using y1000.Source.Util;

namespace y1000.Source.Control.Dialog;

public partial class Item : Panel
{
    private VFlowContainer _detailsContainer;

    private CenterContainer _iconContainer;
    
    private Label _name;
    
    private Label _price;

    private int _number;

    public event EventHandler<SlotMouseEvent>? Clicked;
    public override void _Ready()
    {
        _detailsContainer = GetNode<VFlowContainer>("DetailsContainer");
        _iconContainer = _detailsContainer.GetNode<CenterContainer>("IconContainer");
        _name = _detailsContainer.GetNode<Label>("Name");
        _price = _detailsContainer.GetNode<Label>("Price");
    }

    public string ItemName => _name.Text;

    public void ToggleHighlight(bool highlight)
    {
        if (highlight)
        {
            AddThemeStyleboxOverride("panel", new StyleBoxFlat()
            {
                BgColor = new Color("787878")
            });
        }
        else
        {
            RemoveThemeStyleboxOverride("panel");
        }
    }

    public override void _GuiInput(InputEvent inputEvent)
    {
        if (inputEvent.IsPressed() &&
            inputEvent is InputEventMouseButton mouseButton &&
            mouseButton.ButtonIndex == MouseButton.Left)
        {
            if (mouseButton.DoubleClick)
                Clicked?.Invoke(this, new SlotMouseEvent(SlotMouseEvent.Type.MOUSE_LEFT_DOUBLE_CLICK));
            else
                Clicked?.Invoke(this, new SlotMouseEvent(SlotMouseEvent.Type.MOUSE_LEFT_CLICK));
        }
    }

    public void SetDetails(string name, Texture2D icon, int iconColor, int price)
    {
        _name.Text = name;
        _price.Text = price.ToString();
        var textureRect = _iconContainer.GetNode<TextureRect>("Icon");
        textureRect.Texture = icon;
        if (iconColor != 0)
        {
            textureRect.Material = DyeShader.CreateShaderMaterial(iconColor);
        }
    }

    public static Item Create()
    {
        return ResourceLoader
            .Load<PackedScene>("res://Scenes/Item.tscn").Instantiate<Item>();
    }
}