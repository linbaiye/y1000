using System.Collections.Generic;
using Godot;
using Google.Protobuf.WellKnownTypes;
using y1000.Source.Entity;
using y1000.Source.Networking.Server;

namespace y1000.Source.Creature;

public class MessageDrivenCreature : ICreature
{
    private readonly ICreature _wrapped;

    private readonly Queue<IEntityMessage> _queue;

    public MessageDrivenCreature(ICreature wrapped)
    {
        _wrapped = wrapped;
        _queue = new();
    }

    public string EntityName => _wrapped.EntityName;
    public long Id => _wrapped.Id;
    public Vector2I Coordinate => _wrapped.Coordinate;
    public Direction Direction => _wrapped.Direction;

    public void Handle(IEntityMessage message)
    {
        //if ()
        
    }

    private void WhenFinished(object? sender, CreatureAnimationDoneEventArgs args)
    {
        
    }

    public static MessageDrivenCreature Wrap(AbstractCreature creature)
    {
        var messageDrivenCreature = new MessageDrivenCreature(creature);
        creature.StateAnimationEventHandler += messageDrivenCreature.WhenFinished;
        return messageDrivenCreature;
    }
}