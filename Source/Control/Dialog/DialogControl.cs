using NLog;
using y1000.Source.Character;
using y1000.Source.Creature.Monster;
using y1000.Source.Event;
using y1000.Source.Sprite;

namespace y1000.Source.Control.Dialog;

public partial class DialogControl : Godot.Control
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    private MerchantControl? _merchantControl;
    private MerchantTrading? _merchantTrading;
    
    private ISpriteRepository _spriteRepository = EmptySpriteRepository.Instance;

    public override void _Ready()
    {
        _merchantControl = GetNode<MerchantControl>("Merchant");
        _merchantControl.SetCallback(OnTradeClicked);
        _merchantTrading = GetNode<MerchantTrading>("MerchantTrading");
    }

    public void Initialize(ISpriteRepository spriteRepository, TradeInputWindow tradeInputWindow, EventMediator eventMediator)
    {
        _spriteRepository = spriteRepository;
        _merchantTrading?.Initialize(tradeInputWindow, eventMediator);
    }

    private void OnTradeClicked(bool sell)
    {
        if (_merchantControl == null || _merchantControl.Merchant == null)
        {
            return;
        }
        _merchantControl.Close();
        if (sell)
        {
            _merchantTrading?.Sell(_merchantControl.Merchant, _spriteRepository);
        }
        else
        {
            _merchantTrading?.Buy(_merchantControl.Merchant, _spriteRepository);
        }
    }

    public void OnMerchantClicked(Merchant merchant)
    {
        _merchantControl?.Popup(merchant, _spriteRepository);
    }

    public void BindCharacter(CharacterImpl character)
    {
        _merchantTrading?.BindInventory(character.Inventory);
    }

    public bool OnInventorySlotClick(ClickInventorySlotEvent slotEvent)
    {
        return _merchantTrading?.OnInventorySlotClick(slotEvent) ?? false;
    }
    
}