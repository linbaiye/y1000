using System;
using System.Collections.Generic;
using Godot;

namespace y1000.Source.Util;

public abstract class AbstractSdbReader
{
    private readonly Dictionary<string, int> _header = new();

    private readonly Dictionary<string, string[]> _items = new();

    protected T Parse<T>(string name, string key, Func<string, T> creator)
    {
        if (!_items.TryGetValue(name, out var item))
        {
            throw new NotImplementedException(name + " does not exist.");
        }

        if (!_header.TryGetValue(key, out var index))
        {
            throw new NotImplementedException(key + " does not exist for item " + name);
        }

        return creator.Invoke(item[index]);
    }

    public bool Contains(string name)
    {
        return _items.ContainsKey(name);
    }

    protected string GetString(string name, string key)
    {
        return Parse(name, key, s => s);
    }
    
    
    protected void Read(string filepath)
    {
        var fileAccess = FileAccess.Open(filepath, FileAccess.ModeFlags.Read);
        var line = fileAccess.GetLine();
        if (line == null)
        {
            throw new NotImplementedException("Item.sdb does not have header.");
        }
        var headers = line.Split(",");
        Dictionary<string, int> header = new Dictionary<string, int>();
        for (int i = 0; i < headers.Length; i++)
        {
            _header.TryAdd(headers[i], i);
        }
        while ((line = fileAccess.GetLine()) != null)
        {
            if (string.IsNullOrEmpty(line))
            {
                break;
            }
            var strings = line.Split(",");
            if (!string.IsNullOrEmpty(strings[0]))
            {
                _items.Add(strings[0], strings);
            }
        }
    }
}