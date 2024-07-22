using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Entity;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.DynamicObject;


public partial class GameDynamicObject : AbstractEntity, IBody, IEntity
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    public IMap Map { get; private set; } = IMap.Empty;

    private List<OffsetTexture> _textures = new();

    private const int FrameDuration = 200;

    private int _total = 0;
    
    public void Delete()
    {
        Map.Free(this);
        QueueFree();
    }

    private int Start { get; set; } = 0;
    
    private int End { get; set; } = 0;
    
    private int Elapsed { get; set; } = 0;

    public void Init(long id, Vector2I coordinate, IMap map, string name,
        List<OffsetTexture> textures, int start, int end, int elapse = 0)
    {
        base.Init(id, coordinate, name);
        Map = map;
        map.Occupy(this);
        Start = start;
        End = end;
        Elapsed = elapse;
        _textures = textures;
        _total = (end - start) * FrameDuration;
    }

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
        var ret = Elapsed / FrameDuration;
        Logger.Debug("Index {0}.", ret);
        return ret;
    }
    
    public override OffsetTexture BodyOffsetTexture => _textures[ComputeTextureIndex()];
    
    protected override void MyEvent(InputEvent inputEvent)
    {
    }
}