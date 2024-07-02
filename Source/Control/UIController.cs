using System;
using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.Bottom;
using y1000.Source.Control.Dialog;
using y1000.Source.Control.RightSide;
using y1000.Source.Creature.Monster;
using y1000.Source.Event;
using y1000.Source.Sprite;

namespace y1000.Source.Control;

public partial class UIController : CanvasLayer
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    
    private BottomControl _bottomControl;
    
    private RightControl _rightControl;

    private DropItemUI _dropItemUi;

    private DialogControl _dialogControl;
    
    private TradeInputWindow _tradeInputWindow;

    private readonly List<Func<ClickInventorySlotEvent, bool>> _inventoryClickActions = new();

    public override void _Ready()
    {
        _bottomControl = GetNode<BottomControl>("BottomUI");
        _rightControl = GetNode<RightControl>("RightSideUI");
        _dropItemUi = GetNode<DropItemUI>("DropItemUI");
        _dialogControl = GetNode<DialogControl>("DialogUI");
        _tradeInputWindow = GetNode<TradeInputWindow>("InputWindow");
        _inventoryClickActions.Add(_dialogControl.OnInventorySlotClick);
        _inventoryClickActions.Add(_bottomControl.OnInventorySlotClick);
        BindButtons();
    }

    public void Initialize(EventMediator eventMediator, ISpriteRepository spriteRepository)
    {
        eventMediator.SetComponent(this);
        eventMediator.SetComponent(_bottomControl);
        _tradeInputWindow.BindEventMediator(eventMediator);
        _dropItemUi.BindEventMediator(eventMediator);
        _dialogControl.Initialize(spriteRepository, _tradeInputWindow, eventMediator);
    }

    public void OnClickInventorySlotEvent(ClickInventorySlotEvent slotEvent)
    {
        foreach (var action in _inventoryClickActions)
        {
            if (action.Invoke(slotEvent))
            {
                break;
            }
        }
    }

    public void DisplayMessage(string message)
    {
        _bottomControl.DisplayMessage(new TextEvent(message));
    }
    

    private void BindButtons()
    {
        _bottomControl.InventoryButton.Pressed += _rightControl.OnInventoryButtonClicked;
        _bottomControl.KungFuButton.Pressed += _rightControl.OnKungFuButtonClicked;
    }
    
    public void BindCharacter(CharacterImpl character)
    {
        _bottomControl.BindCharacter(character);
        _rightControl.BindCharacter(character);
        _dialogControl.BindCharacter(character);
    }

    public void OnMerchantClicked(Merchant merchant)
    {
        _dialogControl.OnMerchantClicked(merchant);
    }
}