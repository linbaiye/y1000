using System.Collections.Generic;
using Source.Networking.Protobuf;
using y1000.Source.Character;
using y1000.Source.Event;

namespace y1000.Source.Networking.Server;

public class PlayerAttributeMessage :  ICharacterMessage , IUiEvent
{
    public int AttackSpeed { get; set; } = 1;
    public int MaxLife { get; set; }= 2;
    public int Avoidance { get; set; }= 3;
    public int MaxPower { get; set; }= 4;
    public int Recovery { get; set; }= 5;
    public int MaxInnerPower { get; set; }= 6;
    public int MaxOuterPower { get; set; }= 8;
    public int MaxEnergy { get; set; }= 10;
    public int BodyDamage { get; set; }= 7;
    public int HeadDamage { get; set; }= 9;
    public int ArmDamage { get; set; }= 11;
    public int LegDamage { get; set; }= 12;
    public int BodyArmor { get; set; }= 13;
    public int HeadArmor { get; set; }= 14;
    public int ArmArmor { get; set; }= 15;
    public int LegArmor { get; set; }= 16;
    public int Age { get; set; }= 17;

    public int Basic => Age / 2;

    private List<string>? _cached;


    private string FormatAttr(int n)
    {
        var first = n / 100;
        var second = (n % 100).ToString("00");
        return first + "." + second;
    }

    public List<string> FormatAttributes()
    {
        if (_cached != null)
        {
            return _cached;
        }
        _cached = new List<string>
        {
            "年齡:  " + FormatAttr(Age),
            "活力:  " + FormatAttr(MaxLife),
            "内功:  " + FormatAttr(MaxInnerPower),
            "外功:  " + FormatAttr(MaxOuterPower),
            "武功:  " + FormatAttr(MaxPower),
            "基本:  " + FormatAttr(Basic),
            "綜合:  " + FormatAttr(MaxLife + MaxInnerPower + MaxOuterPower + MaxPower + Basic),
            "攻速:  " + AttackSpeed,
            "恢復:  " + Recovery,
            "閃躲:  " + Avoidance,
            "身攻:  " + BodyDamage,
            "頭攻:  " + HeadDamage,
            "手攻:  " + ArmDamage,
            "脚攻:  " + LegDamage,
            "身防:  " + BodyArmor,
            "頭防:  " + HeadArmor,
            "手防:  " + ArmArmor,
            "脚防:  " + LegArmor,
        };
        return _cached;
    }

    public void Accept(IServerMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(ICharacterMessageVisitor visitor)
    {
        visitor.Visit(this);
    }

    public static PlayerAttributeMessage FromPacket(PlayerRightClickAttributePacket packet)
    {
        return new PlayerAttributeMessage()
        {
            Age = packet.Age,
            MaxPower = packet.MaxPower,
            MaxInnerPower = packet.MaxInnerPower,
            MaxOuterPower = packet.MaxOuterPower,
            MaxLife = packet.MaxLife,
            AttackSpeed = packet.AttackSpeed,
            Avoidance = packet.Avoidance,
            Recovery = packet.Recovery,
            BodyDamage = packet.BodyDamage,
            HeadDamage = packet.HeadDamage,
            ArmDamage = packet.ArmDamage,
            LegDamage = packet.LegDamage,
            BodyArmor = packet.BodyArmor,
            HeadArmor = packet.HeadArmor,
            ArmArmor = packet.ArmArmor,
            LegArmor = packet.LegArmor,
        };
    }
}