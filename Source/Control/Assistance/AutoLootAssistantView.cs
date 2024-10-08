using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Assistant;

namespace y1000.Source.Control.Assistance;

public partial class AutoLootAssistantView : NinePatchRect
{
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    private ItemList _lootItems;
    private ItemList _visibleItems;
    private LineEdit _input;
    private CheckBox _auto;
    private CheckBox _reverse;
    private Button _add;
    private AutoLootAssistant? _assistant;

    private const double Interval = 0.1f;
    private double _timer = Interval;

    public override void _Ready()
    {
        _lootItems = GetNode<ItemList>("LootItems");
        _visibleItems = GetNode<ItemList>("SeeItems");
        _visibleItems.ItemClicked += OnVisibleItemClicked;
        _input = GetNode<LineEdit>("LineEdit");
        _auto = GetNode<CheckBox>("AutoCheckBox");
        _auto.Pressed += () => _assistant?.OnAutoCheckboxChanged(_auto.ButtonPressed);
        _reverse = GetNode<CheckBox>("ReverseCheckBox");
        _reverse.Pressed += () => _assistant?.OnReverseCheckboxChanged(_reverse.ButtonPressed);
        _add = GetNode<Button>("AddButton");
        _lootItems.ItemClicked += OnLootItemSelected;
        _add.Pressed += OnAddClicked;
    }


    private void OnVisibleItemClicked(long idx, Vector2 pos, long btnIdx) {
        if ((btnIdx & (int)MouseButtonMask.Right) != 0)
        {
            var itemName = _visibleItems.GetItemText((int)idx);
            if (!string.IsNullOrEmpty(itemName)) {
                _assistant?.OnAddItemClicked(itemName);
                RefreshLoot();
            }
        }
    }

    private void OnAddClicked()
    {
        if (string.IsNullOrEmpty(_input.Text) || string.IsNullOrEmpty(_input.Text.Trim()))
            return;
        var item = _input.Text.Trim();
        _assistant?.OnAddItemClicked(item);
        RefreshLoot();
        /*_input.Clear();
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
        _assistant?.OnLootItemsChanged(items);*/
    }

    private void RefreshLoot() {
        _lootItems.Clear();
        _assistant?.Loot.ForEach(i => {
            int idx = _lootItems.AddItem(i);
            _lootItems.SetItemTooltipEnabled(idx, false);
        });
    }


    private void OnLootItemSelected(long idx, Vector2 pos, long btnIdx)
    {
        if ((btnIdx & (int)MouseButtonMask.Right) != 0)
        {
            var itemName = _lootItems.GetItemText((int)idx);
            if (string.IsNullOrEmpty(itemName))
                return;
            _assistant?.OnRemoveLootItemClicked(itemName);
            RefreshLoot();
        }
    }


    private void RefreshVisible() {
        _visibleItems.Clear();
        _assistant?.VisibleItems.ForEach(name => {
            int idx = _visibleItems.AddItem(name);
            _visibleItems.SetItemTooltipEnabled(idx, false);
        });
    }


    public override void _Process(double delta)
    {
        _timer -= delta;
        if (_timer > 0)
            return;
        _timer = Interval;
        RefreshVisible();
    }


    public void OnOpen() {
        Visible = true;
        if (_assistant == null)
            return;
        RefreshLoot();
        RefreshVisible();
    }

    public void OnClose() {
        Visible = false;
    }

    public void Bind(AutoLootAssistant? autoLootAssistant)
    {
        _assistant = autoLootAssistant;
        if (_assistant != null)
        {
            _auto.ButtonPressed = _assistant.Auto;
            _reverse.ButtonPressed = _assistant.Reverse;
        }
    }
}