using Godot;
using y1000.Source.Character;
using y1000.Source.Util;

namespace y1000.Source.Control.Assistance;

public partial class HealItemView : NinePatchRect
{
    private CheckBox _checkBox;

    private LineEdit _lineEdit;
    
    private OptionButton _optionButton;

    private CharacterInventory? _inventory;

    public override void _Ready()
    {
        _checkBox = GetNode<CheckBox>("CheckBox");
        _lineEdit = GetNode<LineEdit>("LineEdit");
        _optionButton = GetNode<OptionButton>("OptionButton");
        _optionButton.Clear();
        _optionButton.Pressed += OnButtonClicked;
    }


    private void OnButtonClicked()
    {
        if (_inventory == null)
        {
            return;
        }
        _optionButton.Clear();
        _inventory.Foreach((slot, item) => _optionButton.AddItem(item.ItemName, slot));
    }

    public void Disable()
    {
        _checkBox.Disabled = true;
        _lineEdit.Text = "-";
        _lineEdit.Editable = false;
        _optionButton.Disabled = true;
    }

    public bool Checked()
    {
        return _checkBox.ButtonPressed;
    }

    private bool IsInputValid()
    {
        var str = _lineEdit.Text;
        return str != null && str.DigitOnly();
    }

    public int? Percentage()
    {
        return IsInputValid() ? _lineEdit.Text.ToInt() : null;
    }

    public void SetValues(bool e, int value, string? itemName)
    {
        if (e)
            _checkBox.ButtonPressed = true;
        _lineEdit.Text = value.ToString();
        if (itemName != null)
        {
            _optionButton.Text = itemName;
        }
    }

    public string? BondName => _optionButton.Text;

    public void BindInventory(CharacterInventory inventory)
    {
        _inventory = inventory;
    }
}