using Godot;
using Source.Networking.Protobuf;
using y1000.Source.KungFu.Attack;
using y1000.Source.KungFu.Foot;

namespace y1000.Source.Networking.Server
{
    public class JoinedRealmMessage  : IServerMessage
    {
        public Vector2I Coordinate { get; init; }
        
        public long Id { get; private init; }
        
        public bool Male { get; private init; }
        
        public IFootKungFu? FootKungFu { get; private init; }
        
        public IAttackKungFu? AttackKungFu { get; private init; }

        public static JoinedRealmMessage FromPacket(LoginPacket loginPacket)
        {
            return new JoinedRealmMessage()
            {
                Coordinate = new Vector2I(loginPacket.X, loginPacket.Y),
                Id = loginPacket.Id,
                Male = true,
                FootKungFu = IFootKungFu.ByName(loginPacket.FootKungFuName, loginPacket.FootKungFuLevel),
                AttackKungFu = IAttackKungFu.ByName(loginPacket.AttackKungFuName, loginPacket.AttackKungFuLevel),
            };
        }

        public override string ToString()
        {
            return $"{nameof(Coordinate)}: {Coordinate}";
        }

        public void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}