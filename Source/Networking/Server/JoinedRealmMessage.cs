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
        public class InventoryItemMessage
        {
            public InventoryItemMessage(string name, int slotId, int number)
            {
                Name = name;
                SlotId = slotId;
                Number = number;
            }
            public string Name { get; }
            public int SlotId { get; }
            
            public int Number { get; }
        }
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
        
        public PlayerInfo MyInfo { get; }
        
        private static List<InventoryItemMessage> ItemMessages(LoginPacket loginPacket)
        {
            List<InventoryItemMessage> itemMessages = new List<InventoryItemMessage>();
            foreach (var inventoryItem in loginPacket.InventoryItems)
            {
                itemMessages.Add(new InventoryItemMessage(inventoryItem.Name,  inventoryItem.SlotId, inventoryItem.Number));
            }
            return itemMessages;
        }

        private static IDictionary<int, IKungFu> ParseUnnamedKungFu(LoginPacket packet)
        {
            return ParseKungFu(packet.UnnamedKungFuList);
        }

        private static IDictionary<int, IKungFu> ParseKungFu(RepeatedField<KungFuPacket> kungFuPackets)
        {
            IDictionary<int, IKungFu> result = new Dictionary<int, IKungFu>();
            foreach (var kungFuPacket in kungFuPackets)
            {
                switch (kungFuPacket.Type)
                {
                    case (int)KungFuType.BREATHING:
                        result.TryAdd(kungFuPacket.Slot, new BreathKungFu(kungFuPacket.Name, kungFuPacket.Level));
                        break;
                    case (int)KungFuType.ASSISTANT:
                        result.TryAdd(kungFuPacket.Slot, new AssistantKungFu(kungFuPacket.Name, kungFuPacket.Level));
                        break;
                    case (int)KungFuType.FOOT:
                        result.TryAdd(kungFuPacket.Slot, new Bufa(kungFuPacket.Level, kungFuPacket.Name));
                        break;
                    case (int)KungFuType.PROTECTION:
                        result.TryAdd(kungFuPacket.Slot, new ProtectionKungFu(kungFuPacket.Name, kungFuPacket.Level));
                        break;
                    default:
                        result.TryAdd(kungFuPacket.Slot, IAttackKungFu.ByType((AttackKungFuType)kungFuPacket.Type, kungFuPacket.Name, kungFuPacket.Level));
                        break;
                }
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
            List<InventoryItemMessage> itemMessages = ItemMessages(loginPacket);
            var kungFuBook = CreateKungFuBook(loginPacket);
            var footKungFu = loginPacket.HasFootKungFuName? kungFuBook.FindKungFu<IFootKungFu>(loginPacket.FootKungFuName) : null;
            var attribute = loginPacket.Attribute;
            var message = new JoinedRealmMessage(kungFuBook.FindAttackKungFu(loginPacket.AttackKungFuName),
                itemMessages, PlayerInfo.FromPacket(loginPacket.Info),
                new ValueBar(attribute.CurLife, attribute.MaxLife),
                new ValueBar(attribute.CurPower, attribute.MaxPower),
                new ValueBar(attribute.CurInnerPower, attribute.MaxInnerPower),
                new ValueBar(attribute.CurOuterPower, attribute.MaxOuterPower),
                new ValueBar(attribute.CurEnergy, attribute.MaxEnergy)
            )
            {
                Coordinate = new Vector2I(loginPacket.X, loginPacket.Y),
                FootKungFu = footKungFu,
                KungFuBook = kungFuBook,
                AssistantKungFu = loginPacket.HasAssistantKungFu ? loginPacket.AssistantKungFu : null,
                ProtectionKungFu= loginPacket.HasProtectionKungFu ? loginPacket.ProtectionKungFu: null,
                BreathKungFu = loginPacket.HasBreathKungFu? kungFuBook.FindKungFu<BreathKungFu>(loginPacket.BreathKungFu) : null,
            };
            return message;
        }

        public void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}