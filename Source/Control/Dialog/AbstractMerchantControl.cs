using Godot;
using y1000.Source.Creature.Monster;
using y1000.Source.Sprite;

namespace y1000.Source.Control.Dialog;

public abstract partial class AbstractMerchantControl : NinePatchRect
{
    private Label _nameLabel;
    
    private RichTextLabel _dialog;

    private TextureRect _avatar;

    private Button _close;
    
    public Merchant? Merchant { get; private set; }

    public override void _Ready()
    {
        _nameLabel = GetNode<Label>("Name");
        _dialog = GetNode<RichTextLabel>("Dialog");
        _avatar = GetNode<TextureRect>("Avatar");
        _close = GetNode<Button>("Close");
        _close.Pressed += Close;
        Visible = false;
    }

    public void Close()
    {
        Visible = false;
    }

    protected void Open()
    {
        Visible = true;
    }

    protected void PopulateCommonFields(Merchant merchant, ISpriteRepository spriteRepository, string dialog)
    {
        _nameLabel.Text = merchant.EntityName;
        AtzSprite sprite = spriteRepository.LoadByNpcName(merchant.EntityName);
        _avatar.Texture = sprite.Get(merchant.AvatarSpriteNumber).Texture;
        _dialog.Text = dialog;
        Merchant = merchant;
    }
}