using y1000.Source.KungFu;

namespace y1000.Source.Util;

public class MagicSdbReader : AbstractSdbReader
{
    public static readonly MagicSdbReader Instance = Load();


    public KungFuType GetType(string name)
    {
        return Parse(name, "MagicType", str => (KungFuType)int.Parse(str));
    }

    public string GetEffect(string name)
    {
        return Parse(name, "EffectColor", str => str);
    }

    private static MagicSdbReader Load()
    {
        var magicSdbReader = new MagicSdbReader();
        magicSdbReader.Read("res://assets/sdb/Magic.sdb");
        return magicSdbReader;
    }

}