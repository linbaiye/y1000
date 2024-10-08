
using Source.Networking.Protobuf;
using y1000.Source.Input;
using y1000.Source.KungFu.Attack;

namespace y1000.Source.KungFu;

public class GuildKungFuForm : IClientEvent
{
    public string? Name { get; init; } = "";
    public int Speed {get; init;}
    public int Recovery {get; init;}
    public int Avoid {get; init;}
    public int HeadDamage {get; init;}
    public int ArmDamage {get; init;}
    public int BodyDamage {get; init;}
    public int LegDamage {get; init;}
    public int HeadArmor {get; init;}
    public int ArmArmor {get; init;}
    public int BodyArmor {get; init;}
    public int LegArmor {get; init;}
    public int PowerToSwing {get; init;}
    public int InnerPowerToSwing {get; init;}
    public int OuterPowerToSwing {get; init;}
    public int LifeToSwing {get; init;}
    public AttackKungFuType Type {get; init;}

    public ClientPacket ToPacket()
    {
        return new ClientPacket() {
            CreateGuildKungFu = new ClientCreateGuildKungFuPacket() {
                Name = Name,
                AttackSpeed = Speed,
                Recovery = Recovery,
                Avoidance = Avoid,
                Type = (int)Type,
                BodyDamage = BodyDamage,
                HeadDamage = HeadDamage,
                LegDamage = LegDamage,
                ArmDamage = ArmDamage,
                BodyArmor = BodyArmor,
                HeadArmor = HeadArmor,
                ArmArmor = ArmArmor,
                LegArmor = LegArmor,
                Life = LifeToSwing,
                Power = PowerToSwing,
                InnerPower = InnerPowerToSwing,
                OuterPower = OuterPowerToSwing,
            }
        };
    }


    public string Validate()
    {
        if (string.IsNullOrEmpty(Name)) {
            return "请输入正确名字";
        }
        if (Name.Length > 8) {
            return "名字最长8字符";
        }
        if (Type != AttackKungFuType.AXE && Type != AttackKungFuType.BLADE && Type != AttackKungFuType.SWORD &&
            Type != AttackKungFuType.QUANFA && Type != AttackKungFuType.SPEAR) {
            return "武功只能是刀、剑、拳、槌、枪";
        }
        if (Speed < 1 || Speed > 99) {
            return "速度需在1-99之间";
        }
        if (Recovery < 1 || Recovery > 99) {
            return "恢复需在1-99之间";
        }
        if (Avoid < 1 || Avoid > 99) {
            return "闪躲需在1-99之间";
        }
        if (HeadDamage < 10 || HeadDamage > 70) {
            return "头攻需在10-70之间";
        }
        if (ArmDamage < 10 || ArmDamage > 70) {
            return "手攻需在10-70之间";
        }
        if (BodyDamage < 10 || BodyDamage > 70) {
            return "身攻需在10-70之间";
        }
        if (HeadArmor < 10 || HeadArmor > 70) {
            return "头防需在10-70之间";
        }
        if (ArmArmor < 10 || ArmArmor  > 70) {
            return "手防需在10-70之间";
        }
        if (BodyArmor < 10 || BodyArmor  > 70) {
            return "身防需在10-70之间";
        }
        if (LegArmor < 10 || LegArmor > 70) {
            return "脚防需在10-70之间";
        }
        if (PowerToSwing < 5 || PowerToSwing > 35) {
            return "武功消耗需在5-35之间";
        }
        if (InnerPowerToSwing < 5 || InnerPowerToSwing > 35) {
            return "内功消耗需在5-35之间";
        }
        if (OuterPowerToSwing < 5 || OuterPowerToSwing > 35) {
            return "外功消耗需在5-35之间";
        }
        if (LifeToSwing < 5 || LifeToSwing > 35) {
            return "活力消耗需在5-35之间";
        }
        if (Speed + BodyDamage != 100) {
            return "速度和身攻之和需要等于100";
        }
        if (Recovery  + Avoid != 100) {
            return "恢复和闪躲之和需要等于100";
        }
        if (HeadDamage + ArmDamage + LegDamage  
                + BodyArmor + HeadArmor + ArmArmor 
                + LegArmor != 228) {
            return "头攻+手攻+脚攻+身防+头防+手防+脚防\r\n需要等于228";
        }

        if (OuterPowerToSwing + LifeToSwing + PowerToSwing + InnerPowerToSwing != 80)
        {
            return "外功消耗+内功消耗+武功消耗+活力消耗\r\n需要等于80";
        }

        return "";
    }
    
}