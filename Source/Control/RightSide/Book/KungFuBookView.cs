
using y1000.Source.KungFu;

namespace y1000.Source.Control.RightSide.Book;

public partial class KungFuBookView : AbstractInventoryView
{
    private KungFuBook _kungFuBook = KungFuBook.Empty;

    private int _currentTab = 1;
    
    public override void _Ready()
    {
        ForeachSlot(view => view.OnInputEvent += OnSlotEvent);
    }


    private void OnSlotEvent(object? sender, SlotEvent inputEvent)
    {
        
    }


    public void BindKungFuBook(KungFuBook book)
    {
        _kungFuBook = book;
    }
}