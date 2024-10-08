using Source.Networking.Protobuf;
using y1000.Source.Input;

namespace y1000.Source.Networking;

public class ClientOperateBankEvent : IClientEvent
{
    private enum Operation
    {
        OPEN = 1,
        INVENTORY_TO_BANK = 2,
        BANK_TO_INVENTORY = 3,
        UNLOCK_SLOTS = 4,
        CLOSE = 5,
    }
    
    private Operation Op { get; init; }
    
    private long BankerId { get; init; }
    
    private int FromSlot { get; init; }
    private int ToSlot { get; init; }
    private long Number { get; init; }


    public static ClientOperateBankEvent Open(long bankerId)
    {
        return new ClientOperateBankEvent()
        {
            Op = Operation.OPEN,
            BankerId = bankerId,
        };
    }
    
    public static ClientOperateBankEvent Close()
    {
        return new ClientOperateBankEvent()
        {
            Op = Operation.CLOSE,
        };
    }

    public static ClientOperateBankEvent InventoryToBank(int fromSlot, int toSlot, long number)
    {
        return new ClientOperateBankEvent()
        {
            Op = Operation.INVENTORY_TO_BANK,
            FromSlot = fromSlot,
            ToSlot= toSlot,
            Number = number,
        };
    }
    
    public static ClientOperateBankEvent BankToInventory(int fromSlot, int toSlot, long number)
    {
        return new ClientOperateBankEvent()
        {
            Op = Operation.BANK_TO_INVENTORY,
            FromSlot = fromSlot,
            ToSlot= toSlot,
            Number = number,
        };
    }
        

    public static ClientOperateBankEvent Unlock(int slot)
    {
        return new ClientOperateBankEvent()
        {
            Op = Operation.UNLOCK_SLOTS,
            FromSlot = slot,
        };
    }
    
    public ClientPacket ToPacket()
    {
        return new ClientPacket()
        {
            BankOperation = new ClientBankOperationPacket()
            {
                Type = (int)Op,
                BankerId = BankerId,
                FromSlot = FromSlot,
                ToSlot = ToSlot,
                Number = Number,
            }
        };
    }
}