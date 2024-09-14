using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Assistant;

namespace y1000.Source.Control.Assistance;

public partial class AutoLootAssistantView : NinePatchRect
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    private ItemList _lootItems;
    private ItemList _seeItems;
    private LineEdit _input;
    private CheckBox _auto;
    private CheckBox _reverse;
    private Button _add;
    private AutoLootAssistant? _assistant;

    public override void _Ready()
    {
        _lootItems = GetNode<ItemList>("LootItems");
        _seeItems = GetNode<ItemList>("SeeItems");
        _input = GetNode<LineEdit>("LineEdit");
        _auto = GetNode<CheckBox>("AutoCheckBox");
        _auto.Pressed += () => _assistant?.OnAutoCheckboxChanged(_auto.ButtonPressed);
        _reverse = GetNode<CheckBox>("ReverseCheckBox");
        _reverse.Pressed += () => _assistant?.OnReverseCheckboxChanged(_reverse.ButtonPressed);
        _add = GetNode<Button>("AddButton");
        _lootItems.ItemClicked += OnLootItemSelected;
        _add.Pressed += OnAddClicked;
    }

    private void OnAddClicked()
    {
        if (string.IsNullOrEmpty(_input.Text) || string.IsNullOrEmpty(_input.Text.Trim()))
            return;
        var item = _input.Text.Trim();
        _input.Clear();
        for (int i = 0; i < _lootItems.ItemCount; i++)
        {
            if (_lootItems.GetItemText(i).Equals(item))
                return;
        }
        _lootItems.AddItem(item);
        List<string> items = new List<string>();
        for (int i = 0; i < _lootItems.ItemCount; i++)
        {
            items.Add(_lootItems.GetItemText(i));
        }
        _assistant?.OnLootItemsChanged(items);
    }

    private void OnLootItemSelected(long idx, Vector2 pos, long btnIdx)
    {
        if (btnIdx == (int)MouseButtonMask.Right)
        {
            LOGGER.Debug("Right click on {0}.", idx);
        }
    }

    public void Bind(AutoLootAssistant? autoLootAssistant)
    {
        _assistant = autoLootAssistant;
        if (_assistant != null)
        {
            _auto.ButtonPressed = _assistant.Auto;
            _reverse.ButtonPressed = _assistant.Reverse;
            _assistant.Loot.ForEach(i => _lootItems.AddItem(i));
        }
    }
}