using System.Collections.Generic;
using Godot;
using Google.Protobuf.Collections;
using Source.Networking.Protobuf;
using y1000.Source.KungFu;
using y1000.Source.KungFu.Attack;
using y1000.Source.KungFu.Foot;
using y1000.Source.Util;

namespace y1000.Source.Networking.Server
{
    public class JoinedRealmMessage  : IServerMessage
    {
        private JoinedRealmMessage(IAttackKungFu attackKungFu, List<InventoryItemMessage> items, PlayerInfo myInfo, ValueBar healthBar, 
            ValueBar powerBar, ValueBar innerPowerBar, ValueBar outerPowerBar, ValueBar energyBar)
        {
            AttackKungFu = attackKungFu;
            Items = items;
            MyInfo = myInfo;
            HealthBar = healthBar;
            PowerBar = powerBar;
            InnerPowerBar = innerPowerBar;
            OuterPowerBar = outerPowerBar;
            EnergyBar = energyBar;
            KungFuBook = KungFuBook.Empty;
        }
        
        public List<InventoryItemMessage> Items { get; }
        
        public KungFuBook KungFuBook { get; private set; }
        
        public Vector2I Coordinate { get; private set; }
        
        public IFootKungFu? FootKungFu { get; private init; }
        
        public IAttackKungFu AttackKungFu { get; private init; }
        
        public string? ProtectionKungFu { get; private init; }
        
        public string? AssistantKungFu { get; private init; }
        
        public BreathKungFu? BreathKungFu { get; private init; }
        
        public ValueBar HealthBar { get; private init; }
        public ValueBar PowerBar { get; private init; }
        public ValueBar InnerPowerBar { get; private init; }
        public ValueBar OuterPowerBar { get; private init; }
        
        public ValueBar EnergyBar { get; private init; }
        
        public int HeadPercent { get; private init; }
        public int ArmPercent { get; private init; }
        public int LegPercent { get; private init; }

        public string MapName { get; private init; } = "";
        
        public string TileName { get; private init; } = "";
        
        public string ObjName { get; private init; } = "";
        
        public string RoofName { get; private init; } = "";
        
        public string Bgm { get; private init; } = "";
        
        public string RealmName { get; private init; } = "";
        
        public PlayerInfo MyInfo { get; }
        
        private static IDictionary<int, IKungFu> ParseUnnamedKungFu(LoginPacket packet)
        {
            return ParseKungFu(packet.UnnamedKungFuList);
        }

        private static IDictionary<int, IKungFu> ParseKungFu(RepeatedField<KungFuPacket> kungFuPackets)
        {
            IDictionary<int, IKungFu> result = new Dictionary<int, IKungFu>();
            foreach (var kungFuPacket in kungFuPackets)
            {
                result.TryAdd(kungFuPacket.Slot, IKungFu.Create(kungFuPacket));
            }
            return result;
        }
        
        private static IDictionary<int, IKungFu> ParseBasicKungFu(LoginPacket packet)
        {
            return ParseKungFu(packet.BasicKungFuList);
        }

        private static KungFuBook CreateKungFuBook(LoginPacket packet)
        {
            return new KungFuBook(ParseUnnamedKungFu(packet), ParseBasicKungFu(packet));
        }

        public static JoinedRealmMessage FromPacket(LoginPacket loginPacket)
        {
            List<InventoryItemMessage> itemMessages = InventoryItemMessage.ItemMessages(loginPacket.InventoryItems);
            var kungFuBook = CreateKungFuBook(loginPacket);
            var footKungFu = loginPacket.HasFootKungFuName? kungFuBook.FindKungFu<IFootKungFu>(loginPacket.FootKungFuName) : null;
            var attribute = loginPacket.Attribute;
            var teleport = loginPacket.Teleport;
            var message = new JoinedRealmMessage(kungFuBook.FindAttackKungFu(loginPacket.AttackKungFuName),
                itemMessages, PlayerInfo.FromPacket(loginPacket.Info),
                new ValueBar(attribute.CurLife, attribute.MaxLife),
                new ValueBar(attribute.CurPower, attribute.MaxPower),
                new ValueBar(attribute.CurInnerPower, attribute.MaxInnerPower),
                new ValueBar(attribute.CurOuterPower, attribute.MaxOuterPower),
                new ValueBar(attribute.CurEnergy, attribute.MaxEnergy)
            )
            {
                Coordinate = new Vector2I(teleport.X, teleport.Y),
                FootKungFu = footKungFu,
                KungFuBook = kungFuBook,
                AssistantKungFu = loginPacket.HasAssistantKungFu ? loginPacket.AssistantKungFu : null,
                ProtectionKungFu= loginPacket.HasProtectionKungFu ? loginPacket.ProtectionKungFu: null,
                BreathKungFu = loginPacket.HasBreathKungFu? kungFuBook.FindKungFu<BreathKungFu>(loginPacket.BreathKungFu) : null,
                HeadPercent = attribute.HeadPercent,
                ArmPercent= attribute.ArmPercent,
                LegPercent= attribute.LegPercent,
                MapName = teleport.Map,
                RealmName = teleport.Realm,
                ObjName = teleport.Obj,
                RoofName = teleport.Rof,
                TileName = teleport.Tile,
                Bgm = teleport.Bgm,
            };
            return message;
        }

        public void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}