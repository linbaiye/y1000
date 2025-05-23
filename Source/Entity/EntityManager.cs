﻿using System;
using System.Collections.Generic;
using System.Linq;
using y1000.Source.Creature.Monster;
using y1000.Source.Item;
using y1000.Source.Player;

namespace y1000.Source.Entity;

public class EntityManager
{
    public static readonly EntityManager Instance = new();

    private readonly Dictionary<long, IEntity> _entities = new();
    
    private EntityManager() {}

    public bool Add(IPlayer player)
    {
        return _entities.TryAdd(player.Id, player);
    }

    public IEntity? Remove(long id)
    {
        if (_entities.Remove(id, out var t))
        {
            return t;
        }
        return null;
    }

    public IEnumerable<T> Select<T>(Func<T, bool> filter)
    {
        return _entities.Values.OfType<T>().Where(filter);
    }

    public T? SelectFirst<T>(Func<T, bool> filter)
    {
        return (T?)_entities.Values.FirstOrDefault(e => e is T t && filter.Invoke(t));
    }


    public bool Add(IEntity entity)
    {
        return _entities.TryAdd(entity.Id, entity);
    }

    public IEntity? Get(long id)
    {
        if (_entities.TryGetValue(id, out var t))
        {
            return t;
        }
        return _entities.GetValueOrDefault(id);
    }

    public T? Find<T>(string name)
    {
        return (T?)_entities.Values.FirstOrDefault(c => c.EntityName.Equals(name) && c is T);
    }
    
    public T? Get<T>(long id)
    {
        if (_entities.TryGetValue(id, out var t))
        {
            if (t is T value)
            {
                return value;
            }
        }
        return default;
    }
}