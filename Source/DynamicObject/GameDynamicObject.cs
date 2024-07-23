using System.Collections.Generic;
using System.Linq;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Character;
using y1000.Source.Creature;
using y1000.Source.Entity;
using y1000.Source.Event;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.DynamicObject;


public partial class GameDynamicObject : AbstractEntity, IBody, IEntity, ISlotDoubleClickHandler
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private IMap Map { get; set; } = IMap.Empty;

    private List<OffsetTexture> _textures = new();

    private const int FrameDuration = 200;

    private int _total = 0;

    private CharacterImpl? _character;

    private EventMediator? _eventMediator;

    private void Delete()
    {
        Map.Free(this);
        QueueFree();
    }

    private int Start { get; set; } = 0;
    
    private int End { get; set; } = 0;
    
    private int Elapsed { get; set; } = 0;

    private DynamicObjectType Type { get; set; }

    public IEnumerable<Vector2I> Coordinates { get; private set; } = new HashSet<Vector2I>();

    public void Handle(IEntityMessage message)
    {
        if (message is RemoveEntityMessage)
        {
            Delete();
        }
        else if (message is UpdateDynamicObjectMessage update)
        {
            Start = update.FrameStart;
            End = update.FrameEnd;
            _total = (update.FrameEnd - update.FrameStart) * FrameDuration;
            Elapsed = 0;
        }
    }

    public override void _Process(double delta)
    {
        if (Start == End || Elapsed >= _total)
        {
            return;
        }
        Elapsed += (int)(delta * 1000);
    }

    private int ComputeTextureIndex()
    {
        if (Start == End)
        {
            return Start;
        }
        if (Elapsed >= _total)
        {
            return End;
        }
        return Elapsed / FrameDuration;
    }
    
    public override OffsetTexture BodyOffsetTexture => _textures[ComputeTextureIndex()];
    
    protected override void MyEvent(InputEvent inputEvent)
    {
        // display help.
    }

    public void Initialize(DynamicObjectInterpolation interpolation, IMap map, List<OffsetTexture> texture2Ds)
    {
        Init(interpolation.Id, interpolation.Coordinate, interpolation.Name);
        Map = map;
        Start = interpolation.FrameStart;
        End = interpolation.FrameEnd;
        Elapsed = interpolation.Elapsed;
        _textures = texture2Ds;
        _total = (End - Start) * FrameDuration;
        Coordinates = interpolation.Coordinates;
        Type = interpolation.Type;
        RequiredItem = interpolation.RequiredItem;
        map.Occupy(this);
    }

    private string RequiredItem { get; set; } = "";

    public void BindCharacter(CharacterImpl? character, EventMediator eventMediator)
    {
        if (character != null && Type == DynamicObjectType.TRIGGER)
        {
            _character = character;
            character.Inventory.RegisterRightClickHandler(this);
        }

        _eventMediator = eventMediator;
    }

    public bool HandleInventorySlotDoubleClick(CharacterInventory inventory, int slot)
    {
        if (_character == null || !inventory.HasItem(slot))
        {
            return false;
        }
        var item = inventory.GetOrThrow(slot);
        if (!item.ItemName.Equals(RequiredItem))
        {
            return false;
        }
        var minCoor = Coordinates.MinBy(c => c.Distance(_character.Coordinate));
        if (minCoor.Distance(_character.Coordinate) > 1)
        {
            return false;
        }
        _eventMediator?.NotifyServer(new ClientTriggerDynamicObjectEvent(Id, slot));
        return true;
    }
}