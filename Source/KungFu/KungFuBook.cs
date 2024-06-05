using System;
using System.Collections.Generic;
using y1000.Source.Event;
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

    public void OnUnnamedTabDoubleClick(int nr)
    {
        if (_unnamed.ContainsKey(nr))
        {
            EventMediator?.NotifyServer(new ToggleKungFuEvent(1, nr));
        }
    }
}