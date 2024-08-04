using System;
using y1000.Source.Control.Bottom.Shortcut;

namespace y1000.Source.KungFu;

public class KungFuShortcutEvent : EventArgs
{
    public KungFuShortcutEvent(ShortcutContext context, IKungFu kungFu)
    {
        Context = context;
        KungFu = kungFu;
    }

    public IKungFu KungFu { get; }
    public ShortcutContext Context { get; }
    
}