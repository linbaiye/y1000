
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
    private InventorySlotView? _currentFocused;
    
    
    public override void _Ready()
    {
        base._Ready();
        ForeachSlot(view =>
        {
            view.OnMouseInputEvent += OnSlotEvent;
            view.OnKeyboardEvent += OnSlotKeyEvent;
        });
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

    private void OnSlotKeyEvent(object? sender, SlotKeyEvent keyEvent)
    {
        if (sender is InventorySlotView slotView && _currentPage != null)
            _kungFuBook.OnKeyPressed(_currentPage.Number, slotView.Number, keyEvent.Key);
    }

    private void OnSlotEvent(object? sender, SlotMouseEvent inputMouseEvent)
    {
        if (sender is not InventorySlotView slotView || _currentPage == null)
        {
            return;
        }

        if (inputMouseEvent.EventType == SlotMouseEvent.Type.MOUSE_LEFT_DOUBLE_CLICK)
        {
            _kungFuBook.OnDoubleClick(_currentPage.Number, slotView.Number);
        }
        else if (inputMouseEvent.EventType == SlotMouseEvent.Type.MOUSE_RIGHT_CLICK)
        {
            _kungFuBook.OnRightClick(_currentPage.Number, slotView.Number);
        }
        else if (inputMouseEvent.EventType == SlotMouseEvent.Type.MOUSE_LEFT_RELEASE)
        {
            OnMouseLeftRelease(slotView);
        }
        else if (inputMouseEvent.EventType == SlotMouseEvent.Type.MOUSE_ENTERED)
        {
            _currentFocused = slotView;
        }
        else if (inputMouseEvent.EventType == SlotMouseEvent.Type.MOUSE_GONE)
        {
            _currentFocused = null;
        }
    }
    

    
    private void OnMouseLeftRelease(InventorySlotView picked)
    {
        if (_currentPage == null || _currentFocused == null)
        {
            return;
        }
        if (picked.Number != _currentFocused.Number)
        {
            Log.Debug("Swap drag slots {0} {1}.", picked.Number, _currentFocused.Number);
            _kungFuBook.OnUISwapSlot(_currentPage.Number, picked.Number, _currentFocused.Number);
        }
    }

    private void SetSlotView(int index, IKungFu kungFu)
    {
        var iconId = _magicSdbReader.GetIconId(kungFu.Name);
        var texture2D = _iconReader.Get(iconId);
        if (texture2D != null)
        {
            var slotView = GetSlot(index);
            slotView.PutTextureAndTooltip(texture2D, kungFu.Name + ":" + kungFu.FormatLevel);
        }
    }

    public void RefreshPage()
    {
        ForeachSlot(slot=>slot.Clear());
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
        book.UpdatedEvent += (_, _) => RefreshPage();
    }
    
    public void ButtonClicked()
    {
        Visible = !Visible;
        if (Visible)
            _currentPage?.GrabFocus();
        ToggleMouseFilter();
    }
}