using System;
using System.Text.RegularExpressions;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.RightSide;
using y1000.Source.Control.RightSide.Inventory;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.KungFu;
using y1000.Source.Networking;
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
    private int _viewedSlot;
    private bool _mouseHoveredIcon;
    private bool _viewItem;
    
    public override void _Ready()
    {
        _slotView = GetNode<InventorySlotView>("InventorySlot1");
        _itemName = GetNode<Label>("ItemName");
        _itemDescription = GetNode<RichTextLabel>("ItemDescription");
        _closeButton = GetNode<Button>("CloseButton");
        _closeButton.Pressed += () => { Visible = false; _viewItem = false; };
        _slotView.OnMouseInputEvent += OnSlotMouseEvent;
        Visible = false;
        _mouseHoveredIcon = false;
    }

    private bool DisplayIcon(IItem item)
    {
        var texture2D = _itemIconReader.Get(item.IconId);
        if (texture2D != null)
        {
            _slotView.PutTexture(texture2D, item.Color);
            _itemName.Text = item.ItemName;
            Visible = true;
        }

        return Visible;
    }

    public void Display(ItemAttributeEvent attributeEvent)
    {
        _itemDescription.Text = Regex.Replace(attributeEvent.Description, "<br>", "\n");
        var item = attributeEvent.Item;
        if (DisplayIcon(item)) {
            _viewedSlot = attributeEvent.Slot;
            _viewItem = true;
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
    }

    public void BindCharacter(CharacterImpl character)
    {
         character.Inventory.InventoryChanged += OnInventoryUpdated;
    }

    private void OnInventoryUpdated(object? sender, EventArgs args)
    {
        if (!Visible || !_viewItem)
        {
            return;
        }
        if (sender is not CharacterInventory inventory)
        {
            return;
        }
        var item = inventory.Find(_viewedSlot);
        if (item != null)
        {
            DisplayIcon(item);
        }
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
            _viewItem = false;
        }
    }

    public void Initialize(EventMediator eventMediator)
    {
        _eventMediator = eventMediator;
    }
    
    public bool HandleDragEvent(DragInventorySlotEvent slotEvent)
    {
        if (!Visible || !_mouseHoveredIcon || _viewedSlot == 0)
        {
            return false;
        }
        _eventMediator?.NotifyServer(new ClientDyeEvent(_viewedSlot, slotEvent.Slot));
        return true;
    }
    
}