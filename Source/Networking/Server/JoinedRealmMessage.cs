using System.Collections.Generic;
using Godot;
using Source.Networking.Protobuf;
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
        }
        
        public List<InventoryItemMessage> Items { get; }
        
        public Vector2I Coordinate { get; private set; }
        
        public IFootKungFu? FootKungFu { get; private init; }
        
        public IAttackKungFu AttackKungFu { get; private init; }
        
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

        public static JoinedRealmMessage FromPacket(LoginPacket loginPacket)
        {
            List<InventoryItemMessage> itemMessages = ItemMessages(loginPacket);
            var attackKungFu = IAttackKungFu.ByType((AttackKungFuType)loginPacket.AttackKungFuType, loginPacket.AttackKungFuName, loginPacket.AttackKungFuLevel);
            var footKungFu = loginPacket.HasFootKungFuName
                ? IFootKungFu.ByName(loginPacket.FootKungFuName, loginPacket.FootKungFuLevel)
                : null;
            var message = new JoinedRealmMessage(attackKungFu, itemMessages, PlayerInfo.FromPacket(loginPacket.Info))
            {
                Coordinate = new Vector2I(loginPacket.X, loginPacket.Y),
                FootKungFu = footKungFu,
            };
            return message;
        }

        public void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}