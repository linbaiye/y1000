using System;
using Godot;
using NLog;
using Source.Networking.Protobuf;
using y1000.Source.Networking.Server;

namespace y1000.Source.Creature.Event;

public abstract class AbstractCreatureAttackMessage : AbstractEntityMessage
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    protected AbstractCreatureAttackMessage(long id, Direction direction, CreatureState state, Vector2I coordinate) : base(id)
    {
        Direction = direction;
        State = state;
        Coordinate = coordinate;
    }
    
    public Vector2I Coordinate { get; }
    public Direction Direction { get; }
    
    public CreatureState State { get; }

    public abstract override void Accept(IServerMessageVisitor visitor);

    public override string ToString()
    {
        return $"id: {Id}, {nameof(Coordinate)}: {Coordinate}, {nameof(Direction)}: {Direction}, {nameof(State)}: {State}";
    }

    public static AbstractCreatureAttackMessage FromPacket(CreatureAttackEventPacket packet)
    {
        if (packet.Player)
        {
            var effectId = packet.HasEffectId ? packet.EffectId : 0;
            return new PlayerAttackMessage(packet.Id, (Direction)packet.Direction, (CreatureState)packet.State, new Vector2I(packet.X, packet.Y), effectId);
        }
        var ret = new CreatureAttackMessage(packet.Id, (Direction)packet.Direction, (CreatureState)packet.State, new Vector2I(packet.X, packet.Y));
        Logger.Debug("Received attack message {0}.", ret);
        return ret;
    }
}