using Godot;
using y1000.Source.Creature;
using y1000.Source.Creature.Event;

namespace y1000.Source.Networking.Server;

public class CreatureAttackMessage : AbstractCreatureAttackMessage
{
    public CreatureAttackMessage(long id, Direction direction, CreatureState state, Vector2I coor) : base(id, direction, state, coor)
    {
    }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}