using Source.Networking.Protobuf;
using y1000.Source.KungFu.Attack;
using y1000.Source.KungFu.Foot;

namespace y1000.Source.KungFu;

public interface IKungFu
{
    int Level { get; set; }
    
    string Name { get; }

    public static IKungFu Create(KungFuPacket kungFuPacket)
    {
        switch (kungFuPacket.Type)
        {
            case (int)KungFuType.BREATHING:
                return new BreathKungFu(kungFuPacket.Name, kungFuPacket.Level);
            case (int)KungFuType.ASSISTANT:
                return new AssistantKungFu(kungFuPacket.Name, kungFuPacket.Level);
            case (int)KungFuType.FOOT:
                return new Bufa(kungFuPacket.Level, kungFuPacket.Name);
            case (int)KungFuType.PROTECTION:
                return new ProtectionKungFu(kungFuPacket.Name, kungFuPacket.Level);
            default:
                return IAttackKungFu.ByType((AttackKungFuType)kungFuPacket.Type, kungFuPacket.Name, kungFuPacket.Level);
        }
    }

    string FormatLevel => Level / 100 + "." + (Level% 100).ToString("00");
}