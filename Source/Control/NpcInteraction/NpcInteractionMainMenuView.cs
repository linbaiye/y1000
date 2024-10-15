using Godot;
using y1000.Source.Event;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Control.NpcInteraction;

public partial class NpcInteractionMainMenuView : NinePatchRect
{
    private InteractionGroups _interactionGroups;

    private TextureRect _avatar;

    private Label _text;
    
    private Label _name;

    private long? _currentNpcId;

    private EventMediator? _eventMediator;
    
    public override void _Ready()
    {
        _interactionGroups = GetNode<InteractionGroups>("InteractionGroups");
        _avatar = GetNode<TextureRect>("Avatar");
        _text = GetNode<Label>("Text");
        _name = GetNode<Label>("Name");
        GetNode<Button>("Close").Pressed += () => Visible = false;
        _interactionGroups.OnItemPressed += OnMenuItemClicked;
        Visible = false;
    }

    public void Initialize(EventMediator eventMediator)
    {
        _eventMediator = eventMediator;
    }

    private void OnMenuItemClicked(string name)
    {
        if (_currentNpcId.HasValue)
            _eventMediator?.NotifyServer(new ClientClickInteractabilityEvent(_currentNpcId.Value, name));
        Visible = false;
    }

    public void Handle(NpcInteractionMenuMessage message)
    {
        _text.Text = message.Text;
        _name.Text = message.Name;
        _avatar.Texture = message.Avatar;
        _interactionGroups.Display(message.MenuItems);
        _currentNpcId = message.Id;
        Visible = true;
    }
}