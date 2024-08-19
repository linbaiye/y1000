using System.Text.RegularExpressions;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.RightSide;
using y1000.Source.Control.RightSide.Inventory;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.KungFu;
using y1000.Source.Sprite;
using y1000.Source.Util;

namespace y1000.Source.Control;

public partial class ItemAttributeControl : NinePatchRect
{
    private Label _itemName;
    private RichTextLabel _itemDescription;
    private Button _closeButton;

    private InventorySlotView _slotView;
    
    private readonly IconReader _itemIconReader = IconReader.ItemIconReader;
    private readonly IconReader _kungfuIconReader = IconReader.KungFuIconReader;
    
    private readonly MagicSdbReader _magicSdbReader = MagicSdbReader.Instance;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private EventMediator? _eventMediator;
    private bool _mouseHoveredIcon;
    
    public override void _Ready()
    {
        _slotView = GetNode<InventorySlotView>("InventorySlot1");
        _itemName = GetNode<Label>("ItemName");
        _itemDescription = GetNode<RichTextLabel>("ItemDescription");
        _closeButton = GetNode<Button>("CloseButton");
        _closeButton.Pressed += () => Visible = false;
        _slotView.OnMouseInputEvent += OnSlotMouseEvent;
        Visible = false;
        _mouseHoveredIcon = false;
    }

    public void Display(IItem item, string description)
    {
        _itemDescription.Text = Regex.Replace(description, "<br>", "\n");
        var texture2D = _itemIconReader.Get(item.IconId);
        if (texture2D != null)
        {
            _slotView.PutTexture(texture2D, item.Color);
            _itemName.Text = item.ItemName;
            Visible = true;
        }
    }

    private void OnSlotMouseEvent(object? sender, SlotMouseEvent mouseEvent)
    {
        if (mouseEvent.EventType == SlotMouseEvent.Type.MOUSE_ENTERED)
        {
            _mouseHoveredIcon = true;
        }
        else if (mouseEvent.EventType == SlotMouseEvent.Type.MOUSE_GONE)
        {
            _mouseHoveredIcon = false;
        }
        Logger.Debug("Hovered {0}.", _mouseHoveredIcon);
    }

    public void Display(IKungFu kungFu, string description)
    {
        _itemDescription.Text = Regex.Replace(description, "<br>", "\n");
        var iconId = _magicSdbReader.GetIconId(kungFu.Name);
        var texture2D = _kungfuIconReader.Get(iconId);
        if (texture2D != null)
        {
            _slotView.PutTexture(texture2D);
            _itemName.Text = kungFu.Name;
            Visible = true;
        }
    }

    public void Initialize(EventMediator eventMediator)
    {
        _eventMediator = eventMediator;
    }
    

    public bool HandleDragEvent(DragInventorySlotEvent slotEvent, CharacterImpl character)
    {
        if (!Visible || !_mouseHoveredIcon)
        {
            return false;
        }
        Logger.Debug("Dye item.");
        //_eventMediator.NotifyServer();
        return true;
    }
    
}