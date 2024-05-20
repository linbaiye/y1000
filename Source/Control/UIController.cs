using Godot;
using NLog;
using y1000.Source.Control.RightSide.Inventory;

namespace y1000.Source.Control;

public partial class UIController : CanvasLayer
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    public override void _Ready()
    {
        BindButtons();
    }

    public void BindButtons()
    {
        var inventory = GetNode<Inventory>("RightSideUI/Inventory");
        GetNode<TextureButton>("BottomUI/Container2/InventoryButton").Pressed += inventory.ButtonClicked;
    }

    private void OnInventoryButtonClicked()
    {
        LOGGER.Debug("Clicked.");
    }
}