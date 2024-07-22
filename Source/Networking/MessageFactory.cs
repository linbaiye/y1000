using System;
using Godot;
using Source.Networking.Protobuf;
using y1000.code.networking.message;
using y1000.Source.Character.Event;
using y1000.Source.Creature;
using y1000.Source.Creature.Event;
using y1000.Source.Item;
using y1000.Source.KungFu;
using y1000.Source.Networking.Server;
using y1000.Source.Util;
using TextMessage = y1000.Source.Networking.Server.TextMessage;

namespace y1000.Source.Networking;

public class MessageFactory
{
    private readonly ItemFactory _itemFactory;

    private readonly MagicSdbReader _magicSdbReader;

    public MessageFactory(ItemFactory factory)
    {
        _itemFactory = factory;
        _magicSdbReader = MagicSdbReader.Instance;
    }
    
    private AbstractPositionMessage DecodePositionMessage(PositionPacket positionPacket)
    {
        return (PositionType)positionPacket.Type switch
        {
            PositionType.MOVE => PlayerMoveMessage.FromPacket(positionPacket),
            PositionType.TURN => TurnMessage.FromPacket(positionPacket),
            PositionType.SET => SetPositionMessage.FromPacket(positionPacket),
            PositionType.RUN => RunMessage.FromPacket(positionPacket),
            PositionType.FLY => FlyMessage.FromPacket(positionPacket),
            PositionType.REWIND=> RewindMessage.FromPacket(positionPacket),
            _ => throw new NotSupportedException(),
        };
    }

    private MoveEventResponse DecodeInputResponse(InputResponsePacket packet)
    {
        AbstractPositionMessage positionMessage = DecodePositionMessage(packet.PositionPacket);
        return new MoveEventResponse(packet.Sequence, positionMessage);
    }

    private ShowItemMessage Parse(ShowItemPacket packet)
    {
        return new ShowItemMessage(packet.Id, packet.Name, packet.HasNumber ? packet.Number : 0,
            new Vector2I(packet.CoordinateX, packet.CoordinateY));
    }

    private DropItemMessage Parse(DropItemConfirmPacket packet)
    {
        return new DropItemMessage(packet.Slot, packet.NumberLeft);
    }
    
    private UpdateInventorySlotMessage ParseUpdateSlot(InventoryItemPacket packet)
    {
        ICharacterItem? item = null;
        if (!string.IsNullOrEmpty(packet.Name)) {
            var number = packet.HasNumber ? packet.Number : 0;
            item = _itemFactory.CreateCharacterItem(packet.Name, number);
        }
        return new UpdateInventorySlotMessage(packet.SlotId, item);
    }

    private PlayerUnequipMessage Parse(PlayerUnequipPacket packet)
    {
        return new PlayerUnequipMessage(packet.Id, (EquipmentType)packet.EquipmentType);
    }
    
    private PlayerEquipMessage Parse(PlayerEquipPacket packet)
    {
        return new PlayerEquipMessage(packet.Id, packet.EquipmentName);
    }


    private PlayerToggleKungFuMessage Parse(ToggleKungFuPacket packet)
    {
        int level = packet.HasLevel ? packet.Level : 0;
        KungFuType type = _magicSdbReader.GetType(packet.Name);
        return new PlayerToggleKungFuMessage(packet.Id, packet.Name, level, type, packet.Quietly);
    }

    public IServerMessage Create(Packet packet)
    {
        return packet.TypedPacketCase switch
        {
            Packet.TypedPacketOneofCase.PositionPacket => DecodePositionMessage(packet.PositionPacket),
            Packet.TypedPacketOneofCase.LoginPacket => JoinedRealmMessage.FromPacket(packet.LoginPacket),
            Packet.TypedPacketOneofCase.ResponsePacket => DecodeInputResponse(packet.ResponsePacket),
            Packet.TypedPacketOneofCase.PlayerInterpolation => PlayerInterpolation.FromPacket(
                packet.PlayerInterpolation),
            Packet.TypedPacketOneofCase.CreatureInterpolation => NpcInterpolation.FromPacket(
                packet.CreatureInterpolation),
            Packet.TypedPacketOneofCase.RemoveEntity => new RemoveEntityMessage(packet.RemoveEntity.Id),
            Packet.TypedPacketOneofCase.HurtEventPacket => HurtMessage.FromPacket(packet.HurtEventPacket),
            Packet.TypedPacketOneofCase.AttackEventPacket => AbstractCreatureAttackMessage.FromPacket(
                packet.AttackEventPacket),
            Packet.TypedPacketOneofCase.AttackEventResponsePacket => CharacterAttackEventResponse.FromPacket(
                packet.AttackEventResponsePacket),
            Packet.TypedPacketOneofCase.ChangeStatePacket =>
                ChangeStateMessage.FromPacket(packet.ChangeStatePacket),
            Packet.TypedPacketOneofCase.SwapInventorySlotPacket => SwapInventorySlotMessage.FromPacket(
                packet.SwapInventorySlotPacket),
            Packet.TypedPacketOneofCase.ShowItem => Parse(packet.ShowItem),
            Packet.TypedPacketOneofCase.DropItem => Parse(packet.DropItem),
            Packet.TypedPacketOneofCase.UpdateSlot => ParseUpdateSlot(packet.UpdateSlot),
            Packet.TypedPacketOneofCase.Text => TextMessage.FromPacket(packet.Text),
            Packet.TypedPacketOneofCase.Unequip => Parse(packet.Unequip),
            Packet.TypedPacketOneofCase.Equip => Parse(packet.Equip),
            Packet.TypedPacketOneofCase.ToggleKungFu => Parse(packet.ToggleKungFu),
            Packet.TypedPacketOneofCase.SitDown => new PlayerSitDownMessage(packet.SitDown.Id),
            Packet.TypedPacketOneofCase.StandUp => new PlayerStandUpMessage(packet.StandUp.Id),
            Packet.TypedPacketOneofCase.Cooldown => new PlayerCooldownMessage(packet.Cooldown.Id),
            Packet.TypedPacketOneofCase.Attribute => CharacterAttributeMessage.FromPacket(packet.Attribute),
            Packet.TypedPacketOneofCase.Sound => new CreatureSoundMessage(packet.Sound.Id, packet.Sound.Sound),
            Packet.TypedPacketOneofCase.Die => new CreatureDieMessage(packet.Die.Id, packet.Die.Sound),
            Packet.TypedPacketOneofCase.PlayerRevive => new PlayerReviveMessage(packet.PlayerRevive.Id),
            Packet.TypedPacketOneofCase.GainExp => new GainExpMessage(packet.GainExp.Name, packet.GainExp.Level, packet.GainExp.KungFu),
            Packet.TypedPacketOneofCase.Projectile => ProjectileMessage.FromPacket(packet.Projectile),
            Packet.TypedPacketOneofCase.MonsterMove => new MonsterMoveMessage(packet.MonsterMove.Id, (Direction)packet.MonsterMove.Direction, packet.MonsterMove.Speed),
            Packet.TypedPacketOneofCase.LearnKungFu => PlayerLearnKungFuMessage.FromPacket(packet.LearnKungFu),
            Packet.TypedPacketOneofCase.ItemAttribute => KungFuOrItemAttributeMessage.FromPacket(packet.ItemAttribute),
            Packet.TypedPacketOneofCase.RightClickAttribute => PlayerAttributeMessage.FromPacket(packet.RightClickAttribute),
            Packet.TypedPacketOneofCase.OpenTradeWindow => OpenTradeWindowMessage.FromPacket(packet.OpenTradeWindow),
            Packet.TypedPacketOneofCase.UpdateTradeWindow => UpdateTradeWindowMessage.FromPacket(packet.UpdateTradeWindow),
            Packet.TypedPacketOneofCase.ShowDynamicObject => DynamicObjectInterpolation.FromPacket(packet.ShowDynamicObject),
            Packet.TypedPacketOneofCase.UpdateDynamicObject => new UpdateDynamicObjectMessage(packet.UpdateDynamicObject.Id, 
                packet.UpdateDynamicObject.Start, packet.UpdateDynamicObject.End),
            _ => throw new NotSupportedException()
        };
    }
}