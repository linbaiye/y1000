using Godot;
using y1000.Source.Audio;
using y1000.Source.Event;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.Entity;

public partial class OnGroundItem : Node2D, IEntity, IServerMessageVisitor
{
	private EventMediator? EventMediator { get; set; }
    
    private void Init(long id, string entityName, 
        Vector2I coordinate, Texture2D texture2D)
    {
        Id = id;
        EntityName = entityName;
        Position = coordinate.ToPosition();
        GetNode<Sprite2D>("Sprite").Texture = texture2D;
        GetNode<Panel>("Sprite/Rect").TooltipText = entityName;
        GetNode<Panel>("Sprite/Rect").GuiInput += OnRectInput;
    }
    

    private void OnRectInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton button && (button.DoubleClick || button.IsPressed()) &&
            (button.ButtonMask & MouseButtonMask.Left) != 0)
            EventMediator?.NotifyServer(new PickItemEvent((int)Id));
    }

    public long Id { get; private set; }
    
    public string EntityName { get; private set; } = "";
    
    public Vector2I Coordinate => Position.ToCoordinate();
    

    public static OnGroundItem Create(ShowItemMessage message, Texture2D texture2D, EventMediator eventMediator)
    {
		PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Item.tscn");
		var item = scene.Instantiate<OnGroundItem>();
        var name = message.Number > 0 ? message.Name + ":" + message.Number : message.Name;
        item.Init(message.Id, name, message.Coordinate, texture2D);
        item.EventMediator = eventMediator;
        return item;
    }

    public void Visit(CreatureSoundMessage message)
    {
        GetNode<CreatureAudio>("Audio").PlaySound(message.Sound);
    }


    public void Handle(IEntityMessage message)
    {
        message.Accept(this);
    }
}
