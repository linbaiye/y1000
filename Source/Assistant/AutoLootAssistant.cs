using System.Linq;
using y1000.Source.Character;
using y1000.Source.Entity;
using y1000.Source.Util;

namespace y1000.Source.Assistant;

public class AutoLootAssistant
{
    private readonly EntityManager _entityManager;
    private readonly CharacterImpl _character;

    private const double Interval = 0.5;

    private double _timer;

    private AutoLootAssistant(EntityManager entityManager,
        CharacterImpl character)
    {
        _entityManager = entityManager;
        _character = character;
        _timer = Interval;
    }
    
    public void Process(double d)
    {
        _timer -= d;
        if (_timer > 0)
            return;
        _timer = Interval;
        var groundItems = _entityManager.Select<GroundItem>(item => item.Coordinate.Distance(_character.Coordinate) <= 2);
        groundItems.FirstOrDefault()?.Pick();
    }

    public static AutoLootAssistant Create(EntityManager entityManager,
        CharacterImpl character)
    {
        return new AutoLootAssistant(entityManager, character);
    }
}