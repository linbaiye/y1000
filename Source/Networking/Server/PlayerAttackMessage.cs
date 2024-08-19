using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Creature;
using y1000.Source.Creature.Event;

namespace y1000.Source.Networking.Server;

public sealed class PlayerAttackMessage : AbstractCreatureAttackMessage
{
    
    public PlayerAttackMessage(long id, Direction direction, CreatureState state, Vector2I coor, int effectId) : base(id, direction, state, coor)
    {
        EffectId = effectId;
    }
    
    public int EffectId { get; }
    

    public override void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }


    public override string ToString()
    {
        return "Attack [Id: " + Id + ", State: " + State + "]";
    }
}