using System.Collections.Generic;

namespace y1000.Source.KungFu;

public class KungFuBook
{
    private readonly IDictionary<int, IKungFu> _unnamed ;

    public KungFuBook(IDictionary<int, IKungFu> unnamed)
    {
        _unnamed = unnamed;
    }

    public static readonly KungFuBook Empty = new KungFuBook(new Dictionary<int, IKungFu>());
}