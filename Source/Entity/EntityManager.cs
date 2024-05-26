using System.Collections.Generic;
using y1000.Source.Creature.Monster;
using y1000.Source.Item;
using y1000.Source.Player;

namespace y1000.Source.Entity;

public class EntityManager
{
    public static readonly EntityManager Instance = new();

    private readonly Dictionary<long, IEntity> _entities = new();
    
    private EntityManager() {}

    public void Add(IPlayer player)
    {
        _entities.TryAdd(player.Id, player);
    }

    public IEntity? Remove(long id)
    {
        if (_entities.Remove(id, out var t))
        {
            return t;
        }
        return null;
    }

    public void Add(Monster monster)
    {
        _entities.TryAdd(monster.Id, monster);
    }

    public void Add(GroundedItem item)
    {
        _entities.Add(item.Id, item);
    }

    public void Add(IEntity entity)
    {
        if (entity is GroundedItem item)
        {
            Add(item);
        }
        else
        {
            _entities.TryAdd(entity.Id, entity);
        }
    }

    public IEntity? Get(long id)
    {
        if (_entities.TryGetValue(id, out var t))
        {
            return t;
        }
        return _entities.GetValueOrDefault(id);
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