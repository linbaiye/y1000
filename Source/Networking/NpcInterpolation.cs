using Godot;
using Source.Networking.Protobuf;
using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Creature.Monster;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class NpcInterpolation : IServerMessage
{
    private NpcInterpolation(string name, Interpolation interpolation, long id, NpcType npcType, string textFile, string animate, string shape)
    {
        
        Name = name;
        Interpolation = interpolation;
        Id = id;
        NpcType = npcType;
        TextFile = textFile;
        Animate = animate;
        Shape = shape;
    }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Interpolation)}: {Interpolation}, {nameof(Id)}: {Id}";
    }

    public string Name { get; private set; }
    
    public Interpolation Interpolation { get; }
    
    public long Id { get; }

    public NpcType NpcType { get; }
    
    public string TextFile { get; }

    public string Animate { get; }
    
    public string Shape { get; }
    
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static NpcInterpolation FromPacket(CreatureInterpolationPacket packet)
    {
        return new NpcInterpolation(packet.Name, Interpolation.FromPacket(packet.Interpolation), packet.Id, (NpcType)packet.Type, packet.HasMerchantFile ? packet.MerchantFile : "",
            packet.Animate, packet.Shape);
    }

}