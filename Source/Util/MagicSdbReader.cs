namespace y1000.Source.Util;

public class MagicSdbReader : AbstractSdbReader
{
    public static readonly MagicSdbReader Instance = Load();


    private static MagicSdbReader Load()
    {
        var magicSdbReader = new MagicSdbReader();
        magicSdbReader.Read("res://assets/sdb/Magic.sdb");
        return magicSdbReader;
    }

}