using System;
using Godot;
using Source.Networking.Protobuf;
using y1000.code.networking.message;
using y1000.Source.Character.Event;
using y1000.Source.Creature;
using y1000.Source.Creature.Event;
using y1000.Source.Item;
using y1000.Source.KungFu;
using y1000.Source.KungFu.Attack;
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
            PositionType.MOVE => MoveMessage.FromPacket(positionPacket),
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

    private CharacterChangeWeaponMessage Parse(CharacterChangeWeaponPacket packet)
    {
        ICharacterItem? newItem = null;
        if (packet.HasSlotNewItemName)
        {
            newItem = _itemFactory.CreateCharacterItem(packet.SlotNewItemName);
        }
        IAttackKungFu? attackKungFu = null;
        if (packet.HasAttackKungFuName)
        {
            attackKungFu = IAttackKungFu.ByType((AttackKungFuType)packet.AttackKungFuType, packet.AttackKungFuName, packet.AttackKungFuLevel);
        }
        return new CharacterChangeWeaponMessage(packet.Name, packet.AffectedSlot, newItem, attackKungFu, (CreatureState)packet.State);
    }

    private PlayerChangeWeaponMessage Parse(ChangeWeaponPacket packet)
    {
        return new PlayerChangeWeaponMessage(packet.Id, packet.Name, (CreatureState)packet.State);
    }

    private ShowItemMessage Parse(ShowItemPacket packet)
    {
        return new ShowItemMessage(packet.Id, packet.Name, packet.HasNumber ? packet.Number : 0,
            new Vector2I(packet.CoordinateX, packet.CoordinateY),
            new Vector2(packet.X, packet.Y));
    }

    private TextMessage Parse(TextMessagePacket packet)
    {
        return new TextMessage(packet.Text);
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
        CreatureState? st = packet.HasChangedToState ? (CreatureState) packet.ChangedToState : null;
        var level = packet.HasChangedToState ? packet.BasicQuanfaLevel : 0;
        return new PlayerUnequipMessage(packet.Id, (EquipmentType)packet.EquipmentType, level, st);
    }
    
    private PlayerEquipMessage Parse(PlayerEquipPacket packet)
    {
        return new PlayerEquipMessage(packet.Id, packet.EquipmentName);
    }


    private PlayerToggleKungFuMessage Parse(ToggleKungFuPacket packet)
    {
        int level = packet.HasLevel ? packet.Level : 0;
        KungFuType type = _magicSdbReader.GetType(packet.Name);
        return new PlayerToggleKungFuMessage(packet.Id, packet.Name, level, type);
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
            Packet.TypedPacketOneofCase.CreatureInterpolation => CreatureInterpolation.FromPacket(
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
            Packet.TypedPacketOneofCase.CharacterChangeWeaponPacket => Parse(packet.CharacterChangeWeaponPacket),
            Packet.TypedPacketOneofCase.ChangeWeaponPacket => Parse(packet.ChangeWeaponPacket),
            Packet.TypedPacketOneofCase.ShowItem => Parse(packet.ShowItem),
            Packet.TypedPacketOneofCase.DropItem => Parse(packet.DropItem),
            Packet.TypedPacketOneofCase.UpdateSlot => ParseUpdateSlot(packet.UpdateSlot),
            Packet.TypedPacketOneofCase.Text => Parse(packet.Text),
            Packet.TypedPacketOneofCase.Unequip => Parse(packet.Unequip),
            Packet.TypedPacketOneofCase.Equip => Parse(packet.Equip),
            Packet.TypedPacketOneofCase.ToggleKungFu => Parse(packet.ToggleKungFu),
            Packet.TypedPacketOneofCase.SitDown => new PlayerSitDownMessage(packet.SitDown.Id),
            Packet.TypedPacketOneofCase.StandUp => new PlayerStandUpMessage(packet.StandUp.Id),
            _ => throw new NotSupportedException()
        };
    }
}