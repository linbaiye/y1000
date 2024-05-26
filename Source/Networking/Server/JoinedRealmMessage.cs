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
        private JoinedRealmMessage(IAttackKungFu attackKungFu, List<InventoryItemMessage> items, string name)
        {
            AttackKungFu = attackKungFu;
            Items = items;
            Name = name;
        }
        
        public List<InventoryItemMessage> Items { get; }
        
        public string? WeaponName { get; private set; }

        public Vector2I Coordinate { get; init; }
        
        public long Id { get; private init; }
        
        public bool Male { get; private init; }
        
        public IFootKungFu? FootKungFu { get; private init; }
        
        public IAttackKungFu AttackKungFu { get; private init; }
        
        public string Name { get; }
        
        public string? ChestName { get; private set; }
        
        public string? HatName { get; private set; }


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
            var message = new JoinedRealmMessage(attackKungFu, itemMessages, loginPacket.Name)
            {
                Coordinate = new Vector2I(loginPacket.X, loginPacket.Y),
                Id = loginPacket.Id,
                Male = loginPacket.Male,
                FootKungFu = footKungFu,
                HatName = loginPacket.HasHatName? loginPacket.HatName : null,
                ChestName= loginPacket.HasChestName? loginPacket.ChestName: null,
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