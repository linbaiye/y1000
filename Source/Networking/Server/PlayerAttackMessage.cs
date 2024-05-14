using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature;
using y1000.Source.Creature.Event;

namespace y1000.Source.Networking.Server;

public sealed class PlayerAttackMessage : AbstractCreatureAttackMessage
{
    public PlayerAttackMessage(long id, Direction direction, CreatureState state) : base(id, direction, state)
    {
    }

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
    
}