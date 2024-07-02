using System.Collections.Generic;
using Source.Networking.Protobuf;
using y1000.Source.Character;

namespace y1000.Source.Networking.Server;

public class TextMessage : IServerMessage
{
    
    public enum Type
    {
        FARAWAY = 1,

        CANT_ATTACK = 2,

        INVENTORY_FULL = 3,

        TRADE_REJECTED = 4,

        NO_WEAPON = 5,
        
        NOT_ENOUGH_LIFE = 6,

        NOT_ENOUGH_POWER = 7,

        NOT_ENOUGH_INNER_POWER = 8,

        NOT_ENOUGH_OUTER_POWER = 9,
        
        NOT_ENOUGH_ARM_LIFE = 10,
        
        OUT_OF_AMMO = 11,
        
        CUSTOM = 1000000
        ,
    }

    private static readonly Dictionary<Type, string> TYPE_MAP = new()
    {
        { Type.FARAWAY, "距离过远。" },
        { Type.CANT_ATTACK, "无法攻击。" },
        { Type.INVENTORY_FULL, "物品栏已满。" },
        { Type.TRADE_REJECTED, "对方拒绝交易。" },
        { Type.NO_WEAPON, "没有对应的武器。" },
        { Type.NOT_ENOUGH_LIFE, "活力不足。" },
        { Type.NOT_ENOUGH_POWER, "武功不足。" },
        { Type.NOT_ENOUGH_INNER_POWER, "内功不足。" },
        { Type.NOT_ENOUGH_OUTER_POWER, "外功不足。" },
        { Type.NOT_ENOUGH_ARM_LIFE, "因攻击力过弱而没能获得经验。" },
        { Type.OUT_OF_AMMO, "没有弹药了。" },
    };

    public static TextMessage FromPacket(TextMessagePacket packet)
    {
        if ((Type)packet.Type != Type.CUSTOM)
        {
            return new TextMessage(TYPE_MAP.GetValueOrDefault((Type)packet.Type, ""));
        }

        return new TextMessage(packet.Text);
    }
    
    
    public TextMessage(string text)
    {
        Text = text;
    }

    public string Text { get; }
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }
}