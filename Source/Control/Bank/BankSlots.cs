using Godot;
using y1000.Source.Character;
using y1000.Source.Control.RightSide.Inventory;
using y1000.Source.Sprite;

namespace y1000.Source.Control.Bank;

public partial class BankSlots : GridContainer
{
    private InventorySlotView[] _slotViews = new InventorySlotView[40];
    
    private IconReader _iconReader = IconReader.ItemIconReader;
    
    public override void _Ready()
    {
        for (int i = 0; i < _slotViews.Length; i++)
        {
            _slotViews[i] = GetNode<InventorySlotView>("Slot" + (i + 1));
        }
    }

    public void Display(CharacterBank bank)
    {
        bank.ForeachSlot((slot, item) =>
        {
            var texture2D = _iconReader.Get(item.IconId);
            if (texture2D != null)
                _slotViews[slot - 1].PutTexture(texture2D, item.Color);
        });
        if (bank.Unlocked == bank.Capacity)
        {
            return;
        }
        for (int i = bank.Unlocked; i < bank.Capacity; i++)
        {
            if (ResourceLoader.Load("res://assets/ui/lock.png") is Texture2D texture2D)
                _slotViews[i].PutTexture(texture2D);
        }
    }
    
}