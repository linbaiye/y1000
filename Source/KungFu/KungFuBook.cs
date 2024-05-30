using System;
using System.Collections.Generic;
using Godot.NativeInterop;

namespace y1000.Source.KungFu;

public class KungFuBook
{
    private readonly IDictionary<int, IKungFu> _unnamed ;

    public KungFuBook(IDictionary<int, IKungFu> unnamed)
    {
        _unnamed = unnamed;
    }

    public static readonly KungFuBook Empty = new KungFuBook(new Dictionary<int, IKungFu>());

    public IKungFu? GetUnanamed(int nr)
    {
        return _unnamed.TryGetValue(nr, out var kungfu) ? kungfu : null;
    }

    public void ForeachUnnamed(Action<int, IKungFu> action)
    {
        foreach (var keyValuePair in _unnamed)
        {
            action.Invoke(keyValuePair.Key, keyValuePair.Value);
        }
    }
}