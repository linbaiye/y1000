using System;
using Godot;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character.Event;

public class CharacterTeleportedArgs : EventArgs
{
    public CharacterTeleportedArgs(TeleportMessage message)
    {
        NewMap = message.Map;
        Tile = message.TileName;
        Object = message.ObjName;
        Roof = message.RoofName;
        Realm = message.RealmName;
        Bgm = message.Bgm;
        Coordinate = message.Coordinate;
    }
    
    
    public Vector2I Coordinate { get; }
    
    public string NewMap { get; }
    
    public string Tile { get; }
    public string Object { get; }
    public string Roof { get; }
    
    public string Realm{ get; }
    public string Bgm { get; }
}