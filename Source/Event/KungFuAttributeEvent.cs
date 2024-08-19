using y1000.Source.KungFu;

namespace y1000.Source.Event;

public class KungFuAttributeEvent : IUiEvent
{
    public KungFuAttributeEvent(IKungFu kungFu, string description)
    {
        Description = description;
        KungFu = kungFu;
    }

    public IKungFu KungFu { get; }
    public string Description { get; }
    
}