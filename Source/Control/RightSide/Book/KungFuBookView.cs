
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

    private int _currentTab = 1;
    
    public override void _Ready()
    {
        base._Ready();
        ForeachSlot(view => view.OnInputEvent += OnSlotEvent);
    }

    private void OnSlotEvent(object? sender, SlotEvent inputEvent)
    {
        
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

    public void BindKungFuBook(KungFuBook book)
    {
        _kungFuBook = book;
        _kungFuBook.ForeachUnnamed(SetSlotView);
        //ForeachSlot(SetSlowView);
    }
}