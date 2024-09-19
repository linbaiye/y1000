using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Godot;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Control;
using y1000.Source.KungFu;
using y1000.Source.Storage;

namespace y1000.Source.Assistant;

public class Hotkeys
{
    
    private readonly IDictionary<Key, ShortcutContext> _shortcutContexts;

    public event Action<Hotkeys>? KeyUpdated;

    private CharacterInventory? _inventory;
    private KungFuBook? _kungFuBook;
    private readonly FileStorage _fileStorage;
    private const string FileName = "hotkey";

    public Hotkeys(IDictionary<Key, ShortcutContext> shortcutContexts,
        FileStorage fileStorage)
    {
        _shortcutContexts = shortcutContexts;
        _fileStorage = fileStorage;
    }

    private bool IsAllowedKey(Key key)
    {
        return key is >= Key.F5 and <= Key.F12;
    }

    private void OnInventoryKeyEvent(object? sender, InventoryShortcutEvent shortcutEvent)
    {
        if (sender is CharacterInventory)
        {
            UpdateKey(shortcutEvent.Context);
        }
    }

    private void UpdateKey(ShortcutContext context)
    {
        if (!IsAllowedKey(context.Key))
            return;
        _shortcutContexts.Remove(context.Key);
        foreach (var ctx in _shortcutContexts)
        {
            if (ctx.Value.Slot == context.Slot && ctx.Value.Receiver == context.Receiver)
            {
                _shortcutContexts.Remove(ctx.Key);
                break;
            }
        }
        _shortcutContexts.TryAdd(context.Key, context);
        KeyUpdated?.Invoke(this);
        _fileStorage.Save(FileName, JsonSerializer.Serialize(_shortcutContexts));
    }

    private void OnKungFuBookKeyEvent(object? sender, KungFuShortcutEvent shortcutEvent)
    {
        if (sender is KungFuBook)
        {
            UpdateKey(shortcutEvent.Context);
        }
    }

    public void Handle(Key key)
    {
        if (!_shortcutContexts.TryGetValue(key, out var context))
            return;
        if (context.Receiver == ShortcutContext.Component.KUNGFU_BOOK)
        {
            _kungFuBook?.OnDoubleClick(context.Page, context.Slot);
        }
        else if (context.Receiver == ShortcutContext.Component.INVENTORY)
        {
            _inventory?.OnUIDoubleClick(context.Slot);
        }
    }

    private void BindCharacter(CharacterImpl character)
    {
        character.Inventory.ShortcutEvent += OnInventoryKeyEvent;
        character.KungFuBook.ShortcutPressed += OnKungFuBookKeyEvent;
        _kungFuBook = character.KungFuBook;
        _inventory = character.Inventory;
    }

    private static Hotkeys LoadOrCreate(string charName)
    {
        var fileStorage = new FileStorage(charName);
        var content = fileStorage.Load(FileName);
        if (string.IsNullOrEmpty(content))
        {
            return new Hotkeys(new Dictionary<Key, ShortcutContext>(), fileStorage);
        }
        var shortcutContexts = JsonSerializer.Deserialize<Dictionary<Key, ShortcutContext>>(content);
        if (shortcutContexts == null)
        {
            return new Hotkeys(new Dictionary<Key, ShortcutContext>(), fileStorage);
        }
        return new Hotkeys(shortcutContexts, fileStorage);
    }

    public string GetKeyString(Key key)
    {
        return IsAllowedKey(key) ? "F" + (key - Key.F5 + 5) : "";
    }

    public IEnumerable<ShortcutContext> InventoryContexts =>
        _shortcutContexts.Values.Where(ctx => ctx.Receiver == ShortcutContext.Component.INVENTORY);

    public IEnumerable<ShortcutContext> KungFuContexts =>
        _shortcutContexts.Values.Where(ctx => ctx.Receiver == ShortcutContext.Component.KUNGFU_BOOK);
    
    public static Hotkeys LoadOrCreate(CharacterImpl character)
    {
        var hotkeys = LoadOrCreate(character.EntityName);
        hotkeys.BindCharacter(character);
        return hotkeys;
    }


    public bool CanHandle(Key key)
    {
        return IsAllowedKey(key);
    }
}
