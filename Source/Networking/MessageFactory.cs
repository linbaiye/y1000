using System;
using Source.Networking.Protobuf;
using y1000.code.networking.message;
using y1000.Source.Character.Event;
using y1000.Source.Creature;
using y1000.Source.Creature.Event;
using y1000.Source.Item;
using y1000.Source.KungFu.Attack;
using y1000.Source.Networking.Server;

namespace y1000.Source.Networking;

public class MessageFactory
{
    private readonly ItemFactory _itemFactory;

    public MessageFactory(ItemFactory factory)
    {
        _itemFactory = factory;
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
        var weapon = (CharacterWeapon)_itemFactory.CreateCharacterItem(packet.Name, ItemType.WEAPON);
        ICharacterItem? newItem = null;
        if (packet.HasSlotNewItemName)
        {
            newItem = _itemFactory.CreateCharacterItem(packet.SlotNewItemName, (ItemType)packet.SlotNewItemType);
        }
        IAttackKungFu? attackKungFu = null;
        if (packet.HasAttackKungFuName)
        {
            attackKungFu = IAttackKungFu.ByType((AttackKungFuType)packet.AttackKungFuType, packet.AttackKungFuName, packet.AttackKungFuLevel);
        }
        return new CharacterChangeWeaponMessage(weapon, packet.AffectedSlot, newItem, attackKungFu, (CreatureState)packet.State);
    }

    private PlayerChangeWeaponMessage Parse(ChangeWeaponPacket packet)
    {
        return new PlayerChangeWeaponMessage(packet.Id, _itemFactory.CreatePlayerWeapon(packet.Name), (CreatureState)packet.State);
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
            _ => throw new NotSupportedException()
        };
    }
}