using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using y1000.Source.Control;
using y1000.Source.Control.Bottom.Shortcut;
using y1000.Source.Event;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking;

namespace y1000.Source.KungFu;

public class KungFuBook
{
    private readonly IDictionary<int, IKungFu> _unnamed ;
    
    private readonly IDictionary<int, IKungFu> _basic;

    public EventMediator? EventMediator { get; set; }

    public EventHandler<KungFuShortcutEvent>? ShortcutPressed;
    public EventHandler<EventArgs>? UpdatedEvent;

    public KungFuBook(IDictionary<int, IKungFu> unnamed, IDictionary<int, IKungFu> basic)
    {
        _unnamed = unnamed;
        _basic = basic;
    }

    public static readonly KungFuBook Empty = new(new Dictionary<int, IKungFu>(), new Dictionary<int, IKungFu>());

    public void ForeachUnnamed(Action<int, IKungFu> action)
    {
        foreach (var keyValuePair in _unnamed)
        {
            action.Invoke(keyValuePair.Key, keyValuePair.Value);
        }
    }
    
    public void ForeachBasic(Action<int, IKungFu> action)
    {
        foreach (var keyValuePair in _basic)
        {
            action.Invoke(keyValuePair.Key, keyValuePair.Value);
        }
    }

    private bool KungFuGainExp(IEnumerable<IKungFu> kungFus, string name, int newLevel)
    {
        foreach (var kungFu in kungFus)
        {
            if (kungFu.Name.Equals(name))
            {
                kungFu.Level = newLevel;
                UpdatedEvent?.Invoke(this, EventArgs.Empty);
                return true;
            }
        }
        return false;
    }

    public void Add(int slot, IKungFu kungFu)
    {
        if (_basic.TryAdd(slot, kungFu))
            UpdatedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void GainExp(string name, int newLevel)
    {
        if (KungFuGainExp(_basic.Values, name, newLevel))
        {
            return;
        }
        KungFuGainExp(_unnamed.Values, name, newLevel);
    }

    public T FindKungFu<T>(string name)
    {
        IKungFu? firstOrDefault = _basic.Values.FirstOrDefault(k => k.Name.Equals(name));
        if (firstOrDefault is T kungFu)
        {
            return kungFu;
        }
        firstOrDefault = _unnamed.Values.FirstOrDefault(k => k.Name.Equals(name));
        if (firstOrDefault is T)
        {
            return (T)firstOrDefault;
        }
        throw new KeyNotFoundException(name + " does not exist.");
    }

    public IAttackKungFu FindAttackKungFu(string name)
    {
        return FindKungFu<IAttackKungFu>(name);
    }

    public IKungFu? Get(int page, int nr)
    {
        if (page == 1)
        {
            _unnamed.TryGetValue(nr, out var k);
            return k;
        }
        if (page == 2)
        {
            _basic.TryGetValue(nr, out var k);
            return k;
        }
        return null;
    }

    public void OnRightClick(int page, int nr)
    {
        if (page == 1 || page == 2)
        {
            EventMediator?.NotifyServer(new ClientRightClickEvent(RightClickType.KUNGFU, nr, page));
        }
    }

    public void OnKeyPressed(int page, int nr, Key key)
    {
        var kungFu = Get(page, nr);
        if (kungFu != null)
        {
            ShortcutPressed?.Invoke(this, new KungFuShortcutEvent(ShortcutContext.OfKungFu(page, nr, key), kungFu));
        }
    }

    public void UpdateSlot(int slot, IKungFu? kungFu)
    {
        _basic.Remove(slot);
        if (kungFu != null)
        {
            _basic.TryAdd(slot, kungFu);
        }
        UpdatedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnShortcutPressed(int page, int nr)
    {
        OnDoubleClick(page, nr);
    }

    
    public void OnUISwapSlot(int page, int slot1, int slot2)
    {
        if (page != 2 || slot2 == slot1)
        {
            return;
        }
        EventMediator?.NotifyServer(new ClientSwapKungFuSlotEvent(page, slot1, slot2));
    }

    public void OnDoubleClick(int page, int nr)
    {
        IDictionary<int, IKungFu> kungFus = page == 1 ? _unnamed : _basic;
        if (kungFus.ContainsKey(nr))
        {
            EventMediator?.NotifyServer(new ToggleKungFuEvent(page, nr));
        }
    }
}