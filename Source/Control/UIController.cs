using System;
using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Assistant;
using y1000.Source.Audio;
using y1000.Source.Character;
using y1000.Source.Control.Bank;
using y1000.Source.Control.Bottom;
using y1000.Source.Control.Dialog;
using y1000.Source.Control.LeftSide;
using y1000.Source.Control.Map;
using y1000.Source.Control.PlayerAttribute;
using y1000.Source.Control.PlayerTrade;
using y1000.Source.Control.RightSide;
using y1000.Source.Control.System;
using y1000.Source.Creature.Monster;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.KungFu;
using y1000.Source.Map;
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

    private MapView _mapView;

    private BankView _bankView;

    private SystemMenu _systemMenu;
    
    private SystemSettings _systemSettings;

    private LeftupText _leftupText;
    
    private SystemNotification _systemNotification;
    
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
        _mapView = GetNode<MapView>("MapView");
        _bankView = GetNode<BankView>("Bank");
        _systemMenu = GetNode<SystemMenu>("SysMenu");
        _systemMenu.SettingPressed += OnSysSettingPressed;
        _systemSettings = GetNode<SystemSettings>("SysSetting");
        _leftupText = GetNode<LeftupText>("LeftUpText");
        _systemNotification = GetNode<SystemNotification>("SysNotification");
        BindButtons();
    }

    private void OnSysSettingPressed()
    {
        _systemMenu.Visible = false;
        _systemSettings.Visible = true;
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
        _mapView.Initialize(eventMediator);
        _itemAttributeControl.Initialize(eventMediator);
        _bankView.Initialize(eventMediator, _tradeInputWindow);
    }

    public void DisplayTextMessage(TextMessage message)
    {
        if (message.Location == TextMessage.TextLocation.LEFT)
        {
            _leftsideTextControl.Display(message.Text);
        }
        else if (message.Location == TextMessage.TextLocation.LEFT_UP)
        {
            _leftupText.Display(message.Text);
            _bottomControl.DisplayMessage(new TextEvent(message.Text));
        }
        else if (message.Location == TextMessage.TextLocation.CENTER)
        {
            _systemNotification.Display(message.Text);
        }
        else
        {
            _bottomControl.DisplayMessage(new TextEvent(message.Text, message.ColorType));
        }
    }


    private void BindButtons()
    {
        _bottomControl.InventoryButton.Pressed += _rightControl.OnInventoryButtonClicked;
        _bottomControl.KungFuButton.Pressed += _rightControl.OnKungFuButtonClicked;
        _bottomControl.AssistantButton.Pressed += _rightControl.OnAssistantClicked;
        _bottomControl.SystemButton.Pressed += _systemMenu.OnSystemButtonClicked;
    }
    
    public void BindCharacter(CharacterImpl character,
        string realmName,
        AutoFillAssistant autoFillAssistant,
        AudioManager? audioManager,
        AutoLootAssistant? autoLootAssistant,
        Hotkeys hotkeys)
    {
        _rightControl.BindCharacter(character, autoFillAssistant, autoLootAssistant, hotkeys);
        _bottomControl.BindCharacter(character, realmName);
        _dialogControl.BindCharacter(character);
        _mapView.BindCharacter(character);
        _itemAttributeControl.BindCharacter(character);
        if (audioManager != null)
            _systemSettings.BindAudioManager(audioManager);
    }
    

    public void OnMerchantClicked(Merchant merchant)
    {
        if (IsTrading())
        {
            DisplayTextMessage(TextMessage.MultiTrade);
        }
        else
        {
            _dialogControl.OnMerchantClicked(merchant);
        }
    }

    // Triggered by right-clicking item.
    public void DisplayItemAttribute(ItemAttributeEvent itemAttributeEvent)
    {
        _itemAttributeControl.Display(itemAttributeEvent);
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


    public void DragItem(DragInventorySlotEvent slotEvent, Vector2 mousePosition, CharacterImpl character)
    {
        if (IsTrading())
        {
            DisplayTextMessage(TextMessage.MultiTrade);
            return;
        }
        if (_itemAttributeControl.HandleDragEvent(slotEvent))
        {
            return;
        }
        _dropItemUi.OnDropItem(slotEvent, mousePosition, character.Coordinate);
    }

    public void DropItemOnPlayer(CharacterImpl character, MessageDrivenPlayer messageDrivenPlayer, int slot)
    {
        if (IsTrading())
        {
            DisplayTextMessage(TextMessage.MultiTrade);
            return;
        }
        character.DropItemOnPlayer(messageDrivenPlayer, slot);
    }

    public void OpenTrade(CharacterImpl character, string anotherName, int slot = 0)
    {
        _playerTradeWindow.Open(character, anotherName, slot);
    }

    public void UpdateTrade(UpdateTradeWindowMessage message)
    {
        _playerTradeWindow.Update(message);
    }

    private bool IsTrading()
    {
        return _dialogControl.IsTrading || _dropItemUi.Visible || _bankView.Visible;
    }

    public void ToggleMap(IMap map)
    {
        _mapView.Toggle(map);
    }

    public void DrawNpc(NpcPositionMessage message)
    {
        _mapView.DrawNpcs(message);
    }

    public void OpenBank(CharacterBank bank, CharacterInventory inventory)
    {
        if (IsTrading())
        {
            DisplayTextMessage(TextMessage.MultiTrade);
        }
        else
        {
            _bankView.Open(bank, inventory, _rightControl.InventoryView);
        }
    }

    public void OperateBank(BankOperationMessage message)
    {
        _bankView.Operate(message);
    }
}