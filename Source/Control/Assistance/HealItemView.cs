using Godot;
using y1000.Source.Character;

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

    private bool Checked()
    {
        return _checkBox.ButtonPressed;
    }

    private bool IsInputValid()
    {
        var str = _lineEdit.Text;
        if (str == null)
        {
            return false;
        }
        foreach (char c in str)
        {
            if (c < '0' || c > '9')
                return false;
        }
        var i = str.ToInt();
        return i is > 0 and <= 99;
    }

    public bool IsAutoFillEnabled()
    {
        return IsInputValid() && Checked();
    }

    public int Percentage()
    {
        return IsInputValid() ? _lineEdit.Text.ToInt() : 100;
    }

    public int BondSlot => _optionButton.GetSelectedId() == -1 ? 0 : _optionButton.GetSelectedId();

    public void BindInventory(CharacterInventory inventory)
    {
        _inventory = inventory;
    }
}