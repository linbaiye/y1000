using y1000.Source.Creature;
using y1000.Source.Creature.Event;

namespace y1000.Source.Networking.Server;

public class CreatureAttackMessage : AbstractCreatureAttackMessage
{
    public CreatureAttackMessage(long id, Direction direction) : base(id, direction)
    {
    }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}