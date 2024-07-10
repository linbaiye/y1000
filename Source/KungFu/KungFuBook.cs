using System;
using System.Collections.Generic;
using System.Linq;
using y1000.Source.Event;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking;

namespace y1000.Source.KungFu;

public class KungFuBook
{
    private readonly IDictionary<int, IKungFu> _unnamed ;
    
    private readonly IDictionary<int, IKungFu> _basic;

    public EventMediator? EventMediator { get; set; }

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
                return true;
            }
        }
        return false;
    }

    public void Add(int slot, IKungFu kungFu)
    {
        _basic.TryAdd(slot, kungFu);
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

    public void OnKungFuUsed(int page, int nr)
    {
        if (_unnamed.ContainsKey(nr))
        {
            EventMediator?.NotifyServer(new ToggleKungFuEvent(page, nr));
        }
    }
}