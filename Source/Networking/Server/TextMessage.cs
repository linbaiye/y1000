using System.Collections.Generic;
using Source.Networking.Protobuf;
using y1000.Source.Control.Bottom;

namespace y1000.Source.Networking.Server;

public class TextMessage : IServerMessage
{
    public enum TextLocation
    {
        DOWN = 1,
        LEFT = 2,
        LEFT_UP = 3,
        CENTER = 4,
    }
    
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
        
        NO_MORE_PILL = 12,
        
        MULTI_TRADE = 13,
        
        KUNGFU_LEVEL_LOW = 14,
        
        NINE_TAIL_FOX_SHIFT = 15,
        
        PLAYER_SHOUT = 16,
        
        PLAYER_WHISPER = 18,

        NOT_ENOUGH_HEAD_LIFE = 19,
        
        CUSTOM = 1000000,
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
        { Type.NOT_ENOUGH_HEAD_LIFE, "头部活力过低，无法切换武功。" },
        { Type.OUT_OF_AMMO, "没有弹药了。" },
        { Type.NO_MORE_PILL, "无法再服用。" },
        { Type.MULTI_TRADE, "另一交易正在进行中。" },
        { Type.KUNGFU_LEVEL_LOW, "辅助性武功只能用于满级武功。" },
        { Type.NINE_TAIL_FOX_SHIFT, "<九尾狐变身>：是谁杀了我的妖华？" },
    };

    public static TextMessage FromPacket(TextMessagePacket packet)
    {
        var type = (Type)packet.Type;
        if (TYPE_MAP.TryGetValue(type, out var str))
        {
            return new TextMessage(str, (TextLocation)packet.Location, ColorType.SYSTEM_TIP);
        }
        return new TextMessage(packet.Text, (TextLocation)packet.Location, (ColorType)packet.ColorType);
    }
    
    
    public TextMessage(string text, 
        TextLocation location,
        ColorType colorType = ColorType.SAY)
    {
        Text = text;
        Location = location;
        ColorType = colorType;
    }
    
    public TextLocation Location { get; set; }

    public string Text { get; }
    
    public ColorType ColorType { get; }
    
    
    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static readonly TextMessage MultiTrade = new("另一交易正在进行中。", TextLocation.DOWN, ColorType.SYSTEM_TIP);
}