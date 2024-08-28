using System;
using System.Collections.Generic;
using y1000.Source.Item;
using y1000.Source.Networking.Server;

namespace y1000.Source.Character;

public class CharacterBank : AbstractInventory
{
    public CharacterBank(int unlocked) : base(40)
    {
        Unlocked = unlocked;
    }

    protected override void Notify()
    {
    }


    public bool CanPut(int slot, IItem item)
    {
        if (Unlocked < slot)
        {
            return false;
        }
        var bankItem = Get(slot);
        if (bankItem == null)
        {
            return true;
        }
        return bankItem is CharacterStackItem bankStack
               && item is CharacterStackItem stackItem
               && bankStack.ItemName.Equals(stackItem.ItemName);
    }

    public int Unlocked { get; }


    public static CharacterBank Create(ItemFactory itemFactory, OpenBankMessage message)
    {
        var characterBank = new CharacterBank(message.Unlocked);
        foreach (var inventoryItemMessage in message.ItemMessages)
        {
            var characterItem = itemFactory.CreateCharacterItem(inventoryItemMessage);
            characterBank.PutItem(inventoryItemMessage.SlotId, characterItem);
        }

        return characterBank;
    }

}