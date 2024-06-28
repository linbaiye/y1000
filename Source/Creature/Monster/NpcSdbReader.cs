using y1000.Source.Util;

namespace y1000.Source.Creature.Monster;

public class NpcSdbReader : AbstractSdbReader
{
    public static readonly NpcSdbReader Instance = new();

    private NpcSdbReader()
    {
        Read("res://assets/sdb/Npc.sdb");
    }

    public string GetSpriteName(string monsterName)
    {
        return GetString(monsterName, "shape");
    }

    public string GetAtdName(string monsterName)
    {
        return GetString(monsterName, "animate");
    }
}