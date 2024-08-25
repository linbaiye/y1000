using Godot;
using y1000.Source.Character;

namespace y1000.Source.Control.Bank;

public partial class BankView : NinePatchRect
{
    private BankSlots _slots;
    public override void _Ready()
    {
        _slots = GetNode<BankSlots>("Slots");
    }

    public void Open(CharacterBank bank)
    {
        _slots.Display(bank);
        Visible = true;
    }
}