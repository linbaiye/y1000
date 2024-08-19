using Google.Protobuf.WellKnownTypes;

namespace y1000.Source.Util;

public struct ValueBar
{
    public ValueBar(int current, int max)
    {
        Current = current;
        Max = max;
    }

    private int Current { get; }

    public int Max { get; }

    public int Percent => Current * 100 / Max;

    public string Text => ((float)Current / 100).ToString("0.00") + "/" + ((float)Max / 100).ToString("0.00");

    public static readonly ValueBar Default = new(1, 1);
}