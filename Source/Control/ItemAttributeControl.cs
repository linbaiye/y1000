using Godot;
using y1000.Source.Character;
using y1000.Source.Item;
using y1000.Source.Sprite;

namespace y1000.Source.Control;

public partial class ItemAttributeControl : NinePatchRect
{
    private TextureRect _itemIcon;
    private Label _itemName;
    private RichTextLabel _itemDescription;
    private Button _closeButton;
    private static readonly IconReader ICON_READER = IconReader.ItemIconReader;
    public override void _Ready()
    {
        _itemIcon = GetNode<TextureRect>("ItemIcon/CenterContainer/TextureRect");
        _itemName = GetNode<Label>("ItemName");
        _itemDescription = GetNode<RichTextLabel>("ItemDescription");
        _closeButton = GetNode<Button>("CloseButton");
        _closeButton.Pressed += () => Visible = false;
    }

    public void Display(IItem item, string description)
    {
        _itemDescription.Text = description;
        _itemIcon.Texture = ICON_READER.Get(item.IconId);
        Visible = true;
    }

    public void BindCharacter(CharacterImpl character)
    {
    }
}