using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Character;

namespace y1000.Source.Networking.Server;

public class TeleportMessage: ICharacterMessage
{
    public TeleportMessage(Vector2I coordinate, string map)
    {
        Coordinate = coordinate;
        Map = map;
    }

    public Vector2I Coordinate { get; }
    public string Map { get; }
    
    public string TileName { get; private init; } = "";
        
    public string ObjName { get; private init; } = "";
        
    public string RoofName { get; private init; } = "";
        
    public string Bgm { get; private init; } = "";
        
    public string RealmName { get; private init; } = "";
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static TeleportMessage FromPacket(TeleportPacket teleport)
    {
        return new TeleportMessage(new Vector2I(teleport.X, teleport.Y), teleport.Map)
        {
            RealmName = teleport.Realm,
            ObjName = teleport.Obj,
            RoofName = teleport.Rof,
            TileName = teleport.Tile,
            Bgm = teleport.Bgm,
        };
    }
}