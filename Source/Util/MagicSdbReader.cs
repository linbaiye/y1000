using System;
using System.IO;
using NLog;
using y1000.Source.KungFu;

namespace y1000.Source.Util;

public class MagicSdbReader : AbstractSdbReader
{
    public static readonly MagicSdbReader Instance = Load();
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();



    public KungFuType GetType(string name)
    {
        return Parse(name, "MagicType", str => (KungFuType)int.Parse(str));
    }

    public string GetEffect(string name)
    {
        return Parse(name, "EffectColor", str => str);
    }

    public int GetIconId(string itemName)
    {
        return Parse(itemName, "Shape", int.Parse);
    }

    private static MagicSdbReader Load()
    {
        try {
            var magicSdbReader = new MagicSdbReader();
            magicSdbReader.Read("res://assets/sdb/Magic.sdb");
            return magicSdbReader;
        } catch(Exception e) {
            Logger.Error(e, "Failed to load resource Magic.sdb");
            throw new FileNotFoundException();
        }
    }

}