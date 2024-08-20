using Godot;
using y1000.Source.Control.RightSide;
using y1000.Source.Control.RightSide.Inventory;
using y1000.Source.Event;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.Entity;

public partial class GroundItem  :  Node2D, IEntity, IServerMessageVisitor
{
    private EventMediator? EventMediator { get; set; }

    private InventorySlotView _slotView;

    private Label _name;

    private Texture2D? _texture;

    public override void _Ready()
    {
        _name = GetNode<Label>("Name");
        _slotView = GetNode<InventorySlotView>("Slot1");
        _slotView.OnMouseInputEvent += OnEvent;
        if (_texture != null)
            _slotView.PutTexture(_texture);
    }

    private void Init(long id, string entityName, 
        Vector2I coordinate, Texture2D texture2D)
    {
        Id = id;
        EntityName = entityName;
        Position = coordinate.ToPosition();
        _texture = texture2D;
    }

    private void OnEvent(object? sender, SlotMouseEvent slotMouseEvent)
    {
        if (slotMouseEvent.EventType == SlotMouseEvent.Type.MOUSE_ENTERED)
        {
            _name.Text = EntityName;
            _name.Visible = true;
        } else if (slotMouseEvent.EventType == SlotMouseEvent.Type.MOUSE_GONE)
        {
            _name.Visible = false;
        }
        else if (slotMouseEvent.EventType is SlotMouseEvent.Type.MOUSE_LEFT_DOUBLE_CLICK or SlotMouseEvent.Type.MOUSE_LEFT_CLICK)
        {
            EventMediator?.NotifyServer(new PickItemEvent((int)Id));
        }
    }

    public long Id { get; private set; }
    
    public string EntityName { get; private set; } = "";
    
    public Vector2I Coordinate => Position.ToCoordinate();
    

    public static GroundItem Create(ShowItemMessage message, Texture2D texture2D, EventMediator eventMediator)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/GroundItem.tscn");
        var item = scene.Instantiate<GroundItem>();
        var name = message.Number > 0 ? message.Name + ":" + message.Number : message.Name;
        item.Init(message.Id, name, message.Coordinate, texture2D);
        item.EventMediator = eventMediator;
        return item;
    }

    public void Handle(IEntityMessage message)
    {
        message.Accept(this);
    } 
}