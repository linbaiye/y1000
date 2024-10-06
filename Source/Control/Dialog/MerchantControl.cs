using System;
using System.Linq;
using Godot;
using Godot.NativeInterop;
using y1000.Source.Character.Event;
using y1000.Source.Creature.Monster;
using y1000.Source.Event;
using y1000.Source.Sprite;

namespace y1000.Source.Control.Dialog;

public partial class MerchantControl : AbstractMerchantControl
{

    private Button _buy;
    
    private Button _sell;

    private Button _quest;

    private Action<bool>? _tradingCallback;

    private EventMediator? _eventMediator;

    private Merchant? _currentMerchant;
    
    public override void _Ready()
    {
        base._Ready();
        _sell = GetNode<Button>("Sell");
        _buy = GetNode<Button>("Buy");
        _quest = GetNode<Button>("Quest");
        _sell.Pressed += () => _tradingCallback?.Invoke(true);
        _buy.Pressed += () => _tradingCallback?.Invoke(false);
        _quest.Pressed += OnQuestClicked;
        Close();
    }

    public override void Close()
    {
        Visible = false;
        _quest.Text = "";
        _quest.Visible = false;
        _currentMerchant = null;
    }

    private void OnQuestClicked()
    {
        if (_eventMediator == null || _currentMerchant == null)
        {
            Close();
            return;
        }
        // This will trigger quest, refactor is needed.
        _eventMediator.NotifyServer(new ClickEntityEvent(_currentMerchant.Id));
        Close();
    }

    public void SetCallback(Action<bool> cb)
    {
        _tradingCallback = cb;
    }

    public void Initialize(EventMediator eventMediator)
    {
        _eventMediator = eventMediator;
    }

    public void Popup(Merchant merchant, ISpriteRepository spriteRepository)
    {
        _currentMerchant = merchant;
        PopulateCommonFields(merchant, spriteRepository, "侠士有什么要买卖的吗？");
        _buy.Visible = merchant.SellItems.Any();
        _sell.Visible = merchant.BuyItems.Any();
        if (merchant.QuestName != null)
        {
            _quest.Text = merchant.QuestName;
            _quest.Visible = true;
        }
        Visible = true;
    }
}