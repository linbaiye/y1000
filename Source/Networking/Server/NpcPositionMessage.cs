using System.Collections.Generic;
using Godot;
using Source.Networking.Protobuf;

namespace y1000.Source.Networking.Server;

public class NpcPositionMessage : IServerMessage
{
    public NpcPositionMessage(IEnumerable<NpcPosition> npcPositions)
    {
        NpcPositions = npcPositions;
    }

    public class NpcPosition
    {
        public NpcPosition(Vector2I coordinate, string name)
        {
            Coordinate = coordinate;
            Name = name;
        }

        public Vector2I Coordinate { get; }
        public string Name { get; }
    }
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public IEnumerable<NpcPosition> NpcPositions { get; }
    

    public static NpcPositionMessage FromPacket(NpcPositionPacket packet)
    {
        List<NpcPosition> npcPositions = new List<NpcPosition>();
        for (int i = 0; i < packet.XList.Count; i++)
        {
            var x = packet.XList[i];
            var y = packet.YList[i];
            var name  = packet.NameList[i];
            npcPositions.Add(new NpcPosition(new Vector2I(x, y), name));
        }

        return new NpcPositionMessage(npcPositions);
    }
}