using y1000.Source.Util;

namespace y1000.Source.Creature.Monster;

public class MonsterSdbReader : AbstractSdbReader
{

    public static readonly MonsterSdbReader Instance = new();

    private MonsterSdbReader()
    {
        Read("res://assets/sdb/Monster.sdb");
    }
    

    public string GetSpriteName(string monsterName)
    {
        return GetString(monsterName, "Shape");
    }

    public string GetAtdName(string monsterName)
    {
        return GetString(monsterName, "Animate");
    }
}