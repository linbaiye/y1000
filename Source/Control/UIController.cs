using System;
using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Control.Bottom;
using y1000.Source.Control.Dialog;
using y1000.Source.Control.LeftSide;
using y1000.Source.Control.PlayerAttribute;
using y1000.Source.Control.PlayerTrade;
using y1000.Source.Control.RightSide;
using y1000.Source.Creature.Monster;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.KungFu;
using y1000.Source.Networking.Server;
using y1000.Source.Player;
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
    
    private LeftsideTextControl _leftsideTextControl;
    
    private ItemAttributeControl _itemAttributeControl;

    private PlayerAttributeWindow _attributeWindow;
    
    private PlayerTradeWindow _playerTradeWindow;

    private static readonly string TRADING_ERROR = "另一交易正在进行中。";

    public override void _Ready()
    {
        _bottomControl = GetNode<BottomControl>("BottomUI");
        _rightControl = GetNode<RightControl>("RightSideUI");
        _dropItemUi = GetNode<DropItemUI>("DropItemUI");
        _dialogControl = GetNode<DialogControl>("DialogUI");
        _leftsideTextControl = GetNode<LeftsideTextControl>("LeftsideTextArea");
        _tradeInputWindow = GetNode<TradeInputWindow>("InputWindow");
        _itemAttributeControl = GetNode<ItemAttributeControl>("ItemAttribute");
        _attributeWindow = GetNode<PlayerAttributeWindow>("PlayerAttributeWindow");
        _playerTradeWindow = GetNode<PlayerTradeWindow>("PlayerTradeWindow");
        BindButtons();
    }

    public void Initialize(EventMediator eventMediator,
        ISpriteRepository spriteRepository, ItemFactory itemFactory)
    {
        eventMediator.SetComponent(this);
        eventMediator.SetComponent(_bottomControl);
        _tradeInputWindow.BindEventMediator(eventMediator);
        _dropItemUi.BindEventMediator(eventMediator);
        _dialogControl.Initialize(spriteRepository, _tradeInputWindow, eventMediator, itemFactory);
        _playerTradeWindow.Initialize(_tradeInputWindow, eventMediator);
    }

    public void DisplayTextMessage(TextMessage message)
    {
        if (message.Location == TextMessage.TextLocation.LEFT)
        {
            _leftsideTextControl.Display(message.Text);
        }
        else
        {
            _bottomControl.DisplayMessage(new TextEvent(message.Text));
        }
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
        if (IsTrading())
        {
            DisplayTextMessage(TRADING_ERROR);
        }
        else
        {
            _dialogControl.OnMerchantClicked(merchant);
        }
    }

    // Triggered by right-clicking item.
    public void DisplayItemAttribute(IItem item, string description)
    {
        _itemAttributeControl.Display(item, description);
    }
    
    // Triggered by right-clicking kungfu.
    public void DisplayKungFuAttribute(IKungFu kungFu, string description)
    {
        _itemAttributeControl.Display(kungFu, description);
    }

    // Triggered by right-clicking character avatar.
    public void DisplayCharacterAttributes(List<string> attributes)
    {
        _attributeWindow.DisplayAttributes(attributes);
    }


    private void DisplayTextMessage(string text)
    {
        DisplayTextMessage(new TextMessage(text, TextMessage.TextLocation.DOWN));
    }

    public void DropItem(DragInventorySlotEvent slotEvent, Vector2 mousePosition, Vector2I characterCoordinate)
    {
        if (IsTrading())
        {
            DisplayTextMessage(TRADING_ERROR);
        }
        else
        {
            _dropItemUi.OnDropItem(slotEvent, mousePosition, characterCoordinate);
        }
    }

    public void TradePlayer(CharacterImpl character, MessageDrivenPlayer messageDrivenPlayer, int slot)
    {
        if (IsTrading())
        {
            DisplayTextMessage(new TextMessage("另一交易正在进行中。", TextMessage.TextLocation.DOWN));
            return;
        }
        character.TradeWith(messageDrivenPlayer, slot);
    }

    public void OpenTrade(CharacterImpl character, string anotherName, int slot = 0)
    {
        _playerTradeWindow.Open(character, anotherName, slot);
    }

    public void UpdateTrade(UpdateTradeWindowMessage message)
    {
        _playerTradeWindow.Update(message);
    }

    public bool IsTrading()
    {
        if (_dialogControl.IsTrading)
        {
            return true;
        }
        if (_dropItemUi.Visible)
        {
            return true;
        }
        return false;
    }
}