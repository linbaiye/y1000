using Godot;
using Source.Networking.Protobuf;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class CreatureInterpolation : IServerMessage
{
    private CreatureInterpolation(string name, Interpolation interpolation, long id)
    {
        
        Name = name;
        Interpolation = interpolation;
        Id = id;
        
    }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Interpolation)}: {Interpolation}, {nameof(Id)}: {Id}";
    }

    public string Name { get; private set; }
    
    public Interpolation Interpolation { get; }
    
    public long Id { get; }
    
    public void HandleBy(IServerMessageHandler handler)
    {
        handler.Handle(this);
    }

    public static CreatureInterpolation FromPacket(CreatureInterpolationPacket packet)
    {
        return new CreatureInterpolation(packet.Name, Interpolation.FromPacket(packet.Interpolation), packet.Id);
    }

    public static CreatureInterpolation Mock(long id, Vector2I coordinate)
    {
        return new CreatureInterpolation("牛", new Interpolation(coordinate, CreatureState.IDLE, 0, Direction.LEFT), id);
    }
}