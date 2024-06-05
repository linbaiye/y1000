using System.Collections.Generic;
using Godot;
using Google.Protobuf.Collections;
using Source.Networking.Protobuf;
using y1000.Source.KungFu;
using y1000.Source.KungFu.Attack;
using y1000.Source.KungFu.Foot;

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
        private JoinedRealmMessage(IAttackKungFu attackKungFu, List<InventoryItemMessage> items, PlayerInfo myInfo)
        {
            AttackKungFu = attackKungFu;
            Items = items;
            MyInfo = myInfo;
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
                result.TryAdd(kungFuPacket.Slot, new ViewKungFu(kungFuPacket.Name, kungFuPacket.Level));
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
            var attackKungFu = IAttackKungFu.ByType((AttackKungFuType)loginPacket.AttackKungFuType, loginPacket.AttackKungFuName, 
                loginPacket.AttackKungFuLevel);
            var footKungFu = loginPacket.HasFootKungFuName
                ? IFootKungFu.ByName(loginPacket.FootKungFuName, loginPacket.FootKungFuLevel)
                : null;
            var message = new JoinedRealmMessage(attackKungFu, itemMessages, PlayerInfo.FromPacket(loginPacket.Info))
            {
                Coordinate = new Vector2I(loginPacket.X, loginPacket.Y),
                FootKungFu = footKungFu,
                KungFuBook = CreateKungFuBook(loginPacket),
                AssistantKungFu = loginPacket.HasAssistantKungFu ? loginPacket.AssistantKungFu : null,
                ProtectionKungFu= loginPacket.HasProtectionKungFu ? loginPacket.ProtectionKungFu: null,
                BreathKungFu = loginPacket.HasBreathKungFu? new BreathKungFu(loginPacket.BreathKungFu, loginPacket.BreathKungFuLevel) : null,
            };
            return message;
        }

        public void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}