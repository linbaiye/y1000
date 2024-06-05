
using Godot;
using NLog;
using y1000.Source.Control.RightSide.Inventory;
using y1000.Source.KungFu;
using y1000.Source.Sprite;
using y1000.Source.Util;

namespace y1000.Source.Control.RightSide.Book;

public partial class KungFuBookView : AbstractInventoryView
{
    private KungFuBook _kungFuBook = KungFuBook.Empty;
    
    private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
    
    private readonly IconReader _iconReader = IconReader.KungFuIconReader;
    
    private readonly MagicSdbReader _magicSdbReader = MagicSdbReader.Instance;

    private BookPage? _currentPage;

    
    public override void _Ready()
    {
        base._Ready();
        ForeachSlot(view => view.OnInputEvent += OnSlotEvent);
        var page1 = GetNode<BookPage>("Page1");
        var page2 = GetNode<BookPage>("Page2");
        _currentPage = page1;
        page1.SetCallback(OnPageClicked);
        page2.SetCallback(OnPageClicked);
    }

    private void OnPageClicked(BookPage bookPage)
    {
        _currentPage = bookPage;
        RefreshPage();
    }

    private void OnSlotEvent(object? sender, SlotEvent inputEvent)
    {
        if (sender is not InventorySlotView slotView)
        {
            return;
        }
        switch (inputEvent.EventType)
        {
            case SlotEvent.Type.MOUSE_LEFT_DOUBLE_CLICK:
                _kungFuBook.OnUnnamedTabDoubleClick(slotView.Number);
                break;
        }
    }

    private string KungFuLevel(int level)
    {
        return level / 100 + "." + (level % 100).ToString("00");
    }

    private void SetSlotView(int index, IKungFu kungFu)
    {
        var iconId = _magicSdbReader.GetIconId(kungFu.Name);
        var texture2D = _iconReader.Get(iconId);
        if (texture2D != null)
        {
            var slotView = GetSlot(index);
            slotView.PutTextureAndTooltip(texture2D, kungFu.Name + ":" + KungFuLevel(kungFu.Level));
        }
    }

    private void RefreshPage()
    {
        ForeachSlot(slot=>slot.ClearTexture());
        if (_currentPage?.Number == 1)
        {
            _kungFuBook.ForeachUnnamed(SetSlotView);
        }
        else if (_currentPage?.Number == 2)
        {
            _kungFuBook.ForeachBasic(SetSlotView);
        }
    }

    public void BindKungFuBook(KungFuBook book)
    {
        _kungFuBook = book;
        RefreshPage();
    }
    
    public void ButtonClicked()
    {
        Visible = !Visible;
        if (Visible)
            _currentPage?.GrabFocus();
    }
}