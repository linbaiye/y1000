using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Control.RightSide;
using y1000.Source.Control.RightSide.Inventory;
using y1000.Source.Item;
using y1000.Source.KungFu;
using y1000.Source.Sprite;
using y1000.Source.Storage;
using y1000.Source.Util;

namespace y1000.Source.Control.Bottom.Shortcut;

public partial class Shortcuts : NinePatchRect
{

    private readonly InventorySlotView[] _slots = new InventorySlotView[8];
    
    private IDictionary<int, ShortcutContext> _mappedContext = new Dictionary<int, ShortcutContext>();

    private KungFuBook? _kungFuBook;
    private CharacterInventory? _inventory;

    private const string FileName = "shortcut";

    private long _nextInputAfter;

    private FileStorage? _fileStorage;

    private static readonly IDictionary<Key, int> KEY_TO_INDEX = new Dictionary<Key, int>()
    {
        { Key.Key1, 0},
        { Key.Key2, 1},
        { Key.Key3, 2},
        { Key.Key4, 3},
        { Key.Key5, 4},
        { Key.Key6, 5},
        { Key.Key7, 6},
        { Key.Key8, 7},
    };
    
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private readonly IconReader _kungfuIconReader = IconReader.KungFuIconReader;
    
    private readonly MagicSdbReader _magicSdbReader = MagicSdbReader.Instance;
    
    private readonly IconReader _itemIconReader = IconReader.ItemIconReader;
    
    public override void _Ready()
    {
        for (int i = 1; i <= 8; i++)
        {
            _slots[i - 1] = GetNode<InventorySlotView>("shortcut" + i);
            _slots[i - 1].OnMouseInputEvent += OnSlotMouseEvent;
        }
    }

    private void OnSlotMouseEvent(object? sender, SlotMouseEvent mouseEvent)
    {
        if (sender is not InventorySlotView slot)
        {
            return;
        }
        if (mouseEvent.EventType is SlotMouseEvent.Type.MOUSE_LEFT_CLICK or SlotMouseEvent.Type.MOUSE_LEFT_DOUBLE_CLICK)
        {
            if (_mappedContext.TryGetValue(slot.Number - 1, out var context))
            {
                SendShortcutIfAllowed(context);
            }
        }
    }

    private void SendShortcutIfAllowed(ShortcutContext context)
    {
        var milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        if (milliseconds >= _nextInputAfter)
        {
            if (context.Receiver == Control.ShortcutContext.Component.KUNGFU_BOOK)
            {
                _kungFuBook?.OnShortcutPressed(context.Page, context.Slot);
            }
            else if (context.Receiver == Control.ShortcutContext.Component.INVENTORY)
            {
                _inventory?.OnUIDoubleClick(context.Slot);
            }
            _nextInputAfter = milliseconds + 300;
        }
    }


    public override void _ShortcutInput(InputEvent @event)
    {
        if (@event is InputEventKey key && key.IsPressed() && KEY_TO_INDEX.TryGetValue(key.Keycode, out var index) && 
            _mappedContext.TryGetValue(index, out var context))
        {
            SendShortcutIfAllowed(context);
        }
    }

    private void Restore(string content, KungFuBook kungFuBook, CharacterInventory inventory)
    {
        try
        {
            var ret = JsonSerializer.Deserialize<Dictionary<int, ShortcutContext>>(content);
            if (ret == null)
            {
                return;
            }
            _mappedContext = ret;
            foreach (KeyValuePair<int, ShortcutContext> keyValuePair in _mappedContext)
            {
                var index = keyValuePair.Key;
                var shortcutContext = keyValuePair.Value;
                if (shortcutContext.Receiver == Control.ShortcutContext.Component.KUNGFU_BOOK)
                {
                    var kungFu = kungFuBook.Get(shortcutContext.Page, shortcutContext.Slot);
                    if (kungFu != null)
                    {
                        int iconId = _magicSdbReader.GetIconId(kungFu.Name);
                        var texture2D = _kungfuIconReader.Get(iconId);
                        if (texture2D != null)
                            _slots[index].PutTextureAndTooltip(texture2D, kungFu.Name + ":" + kungFu.FormatLevel);
                    }
                }
                else if (shortcutContext.Receiver == Control.ShortcutContext.Component.INVENTORY)
                {
                    var item = inventory.Get(shortcutContext.Slot);
                    if (item != null)
                    {
                        var texture2D = _itemIconReader.Get(item.IconId);
                        if (texture2D != null)
                            _slots[index].PutTextureAndTooltip(texture2D, item.ItemName);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }


    private void Bind(Texture2D texture2D, string text, int index, ShortcutContext context)
    {
        _slots[index].PutTextureAndTooltip(texture2D, text);
        _mappedContext[index] = context;
        _fileStorage?.Save(FileName, JsonSerializer.Serialize(_mappedContext));
    }

    private void SetKungFuShortcut(object? sender, KungFuShortcutEvent shortcutEvent)
    {
        if (sender is KungFuBook)
        {
            SetKungFu(shortcutEvent.KungFu, shortcutEvent.Context);
        }
    }

    private void SetKungFu(IKungFu? kungFu, ShortcutContext shortcutContext)
    {
        if (!KEY_TO_INDEX.TryGetValue(shortcutContext.Key, out var index))
        {
            return;
        }
        if (kungFu == null)
        {
            _slots[index].ClearTextureAndTip();
            return;
        }
        int iconId = _magicSdbReader.GetIconId(kungFu.Name);
        var texture2D = _kungfuIconReader.Get(iconId);
        if (texture2D != null)
        {
            Bind(texture2D, kungFu.Name + ":" + kungFu.FormatLevel, index, shortcutContext);
        }
    }

    private void SetItem(IItem? item, ShortcutContext context)
    {
        if (!KEY_TO_INDEX.TryGetValue(context.Key, out var index))
        {
            return;
        }
        if (item == null)
        {
            _slots[index].ClearTextureAndTip();
            return;
        }
        var texture2D = _itemIconReader.Get(item.IconId);
        if (texture2D != null)
        {
            Bind(texture2D, item.ItemName, index, context);
        }
    }

    private void SetInventoryShortcut(object? sender, InventoryShortcutEvent shortcutEvent)
    {
        if (sender is CharacterInventory)
        {
            SetItem(shortcutEvent.Item, shortcutEvent.Context);
        }
    }

    private void BindKungFuBook(KungFuBook kungFuBook)
    {
        kungFuBook.ShortcutPressed += SetKungFuShortcut;
        _kungFuBook = kungFuBook;
        _kungFuBook.UpdatedEvent += OnKungFuBookUpdated;
    }

    private void OnKungFuBookUpdated(object? sender, EventArgs eventArgs)
    {
        foreach (var context in _mappedContext.Values)
        {
            if (context.Receiver == Control.ShortcutContext.Component.KUNGFU_BOOK)
            {
                var kungFu = _kungFuBook?.Get(context.Page, context.Slot);
                SetKungFu(kungFu, context);
            }
        }
    }

    private void OnInventoryUpdated(object? sender, EventArgs eventArgs)
    {
        foreach (var context in _mappedContext.Values)
        {
            if (context.Receiver == Control.ShortcutContext.Component.INVENTORY)
            {
                SetItem(_inventory?.Get(context.Slot), context);
            }
        }
    }

    public void Bind(CharacterImpl character)
    {
        BindInventory(character.Inventory);
        BindKungFuBook(character.KungFuBook);
        _fileStorage = new FileStorage(character.EntityName);
        var content = _fileStorage.Load(FileName);
        if (!string.IsNullOrEmpty(content))
        {
            Restore(content, character.KungFuBook, character.Inventory);
        }
    }

    private void BindInventory(CharacterInventory inventory)
    {
        _inventory = inventory;
        inventory.InventoryChanged += OnInventoryUpdated;
        inventory.ShortcutEvent += SetInventoryShortcut;
    }
}