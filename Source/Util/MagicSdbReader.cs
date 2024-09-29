using System;
using System.IO;
using NLog;
using y1000.Source.KungFu;
using y1000.Source.KungFu.Attack;

namespace y1000.Source.Util;

public class MagicSdbReader : AbstractSdbReader
{
    public static readonly MagicSdbReader Instance = Load();
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();



    public KungFuType GetType(string name)
    {
        return Parse(name, "MagicType", str => (KungFuType)int.Parse(str));
    }


    public int GetIconId(string itemName)
    {
        return Parse(itemName, "Shape", int.Parse);
    }

    public int GetIconId(IKungFu kungFu)
    {
        if (Contains(kungFu.Name))
        {
            return GetIconId(kungFu.Name);
        }
        if (kungFu is QuanFa)
        {
            return 4;
        }
        else if (kungFu is SwordKungFu)
        {
            return 19;
        }
        else if (kungFu is BladeKungFu)
        {
            return 35;
        }
        else if (kungFu is SpearKungFu)
        {
            return 51;
        }
        else if (kungFu is AxeKungFu)
        {
            return 67;
        }
        return 0;
    }

    private static MagicSdbReader Load()
    {
        try
        {
            var magicSdbReader = new MagicSdbReader();
            magicSdbReader.Read("res://assets/sdb/Magic.sdb");
            return magicSdbReader;
        }
        catch (Exception e)
        {
            Logger.Error(e, "Failed to load resource Magic.sdb");
            throw new FileNotFoundException();
        }
    }

}