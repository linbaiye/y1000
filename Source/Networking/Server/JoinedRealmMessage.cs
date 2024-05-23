using System.Collections.Generic;
using Godot;
using Source.Networking.Protobuf;
using y1000.Source.Item;
using y1000.Source.KungFu.Attack;
using y1000.Source.KungFu.Foot;

namespace y1000.Source.Networking.Server
{
    public class JoinedRealmMessage  : IServerMessage
    {
        public class InventoryItemMessage
        {
            public InventoryItemMessage(string name, ItemType type, int slotId, int number)
            {
                Name = name;
                Type = type;
                SlotId = slotId;
                Number = number;
            }
            public string Name { get; }
            public ItemType Type { get; }
            public int SlotId { get; }
            
            public int Number { get; }
        }
        private JoinedRealmMessage(IAttackKungFu attackKungFu, List<InventoryItemMessage> items, string name)
        {
            AttackKungFu = attackKungFu;
            Items = items;
            Name = name;
        }
        
        public List<InventoryItemMessage> Items { get; }
        
        public string? WeaponName { get; set; }

        public Vector2I Coordinate { get; init; }
        
        public long Id { get; private init; }
        
        public bool Male { get; private init; }
        
        public IFootKungFu? FootKungFu { get; private init; }
        
        public IAttackKungFu AttackKungFu { get; private init; }
        
        public string Name { get; }


        private static List<InventoryItemMessage> ItemMessages(LoginPacket loginPacket)
        {
            List<InventoryItemMessage> itemMessages = new List<InventoryItemMessage>();
            foreach (var inventoryItem in loginPacket.InventoryItems)
            {
                itemMessages.Add(new InventoryItemMessage(inventoryItem.Name, (ItemType)inventoryItem.ItemType, inventoryItem.SlotId, inventoryItem.Number));
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
            var message = new JoinedRealmMessage(attackKungFu, itemMessages, loginPacket.Name)
            {
                Coordinate = new Vector2I(loginPacket.X, loginPacket.Y),
                Id = loginPacket.Id,
                Male = true,
                FootKungFu = footKungFu,
            };
            if (loginPacket.HasWeaponName)
            {
                message.WeaponName = loginPacket.WeaponName;
            }
            return message;
        }

        public override string ToString()
        {
            return
                $"{nameof(Coordinate)}: {Coordinate}, {nameof(Id)}: {Id}, {nameof(Male)}: {Male}, {nameof(FootKungFu)}: {FootKungFu}, {nameof(AttackKungFu)}: {AttackKungFu}";
        }

        public void Accept(IServerMessageVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}