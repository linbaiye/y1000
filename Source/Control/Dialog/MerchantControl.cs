using System;
using Godot;
using y1000.Source.Creature.Monster;
using y1000.Source.Sprite;

namespace y1000.Source.Control.Dialog;

public partial class MerchantControl : AbstractMerchantControl
{

    private Button _buy;
    
    private Button _sell;

    private Action<bool>? _tradingCallback;
    
    public override void _Ready()
    {
        base._Ready();
        _sell = GetNode<Button>("Sell");
        _buy = GetNode<Button>("Buy");
        _sell.Pressed += () => _tradingCallback?.Invoke(true);
        _buy.Pressed += () => _tradingCallback?.Invoke(false);
        Close();
    }

    public void SetCallback(Action<bool> cb)
    {
        _tradingCallback = cb;
    }
    
    public void Popup(Merchant merchant, ISpriteRepository spriteRepository)
    {
        PopulateCommonFields(merchant, spriteRepository, "侠士有什么要买卖的吗？");
        Visible = true;
    }
}