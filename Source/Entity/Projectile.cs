using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Sprite;
using y1000.Source.Util;

namespace y1000.Source.Entity;

public partial class Projectile : Sprite2D
{
    private static readonly ILogger LOG = LogManager.GetCurrentClassLogger();

    private Vector2 _vect;

    private float _lengthSeconds;

    private float _elapsed;
    
    private float _speed;

    private static readonly Dictionary<Direction, Vector2> INITIAL_POSITION = new()
    {
        { Direction.UP , new Vector2(6, -36) },
        { Direction.RIGHT, new Vector2(29, -19) },
        { Direction.UP_RIGHT, new Vector2(24, -30) },
        { Direction.DOWN_RIGHT, new Vector2(19, -10) },
        { Direction.DOWN, new Vector2(25, 0) },
        { Direction.DOWN_LEFT, new Vector2(6, -9) },
        { Direction.LEFT, new Vector2(1, -21) },
        { Direction.UP_LEFT, new Vector2(12, -32) },
    };
    
    private void Init(int distance, Vector2 start, Vector2 end)
    {
        Position = start;
        _lengthSeconds = distance * 0.03f;
        _vect = end - Position;
        Rotation = _vect.Normalized().Angle();
        _vect /= _lengthSeconds;
    }
    
    private void Init(ICreature shooter, ICreature target)
    {
        Position = shooter.Coordinate.ToPosition() + INITIAL_POSITION.GetValueOrDefault(shooter.Direction);
        var distance = target.Coordinate.Distance(shooter.Coordinate);
        Vector2 centerPoint = target.OffsetBodyPosition + target.BodyOffsetTexture.OriginalSize / 2;
        _lengthSeconds = distance * 0.03f;
        _vect = centerPoint - Position;
        Rotation = _vect.Normalized().Angle();
        _vect /= _lengthSeconds;
    }

    public override void _Process(double delta)
    {
        _elapsed += (float)delta;
        Position += (_vect * (float)delta);
        if (_elapsed >= _lengthSeconds)
        {
            QueueFree();
        }
    }


    
    public static Projectile LoadFor(ICreature shooter, ICreature target, int id)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Arrow.tscn");
        var arrow = scene.Instantiate<Projectile>();
        var atzSprite = FilesystemSpriteRepository.Instance.LoadByNumberAndOffset("y" + id);
        var offsetTexture = atzSprite.Get(20);
        arrow.Texture = offsetTexture.Texture;
        arrow.Init(shooter, target);
        return arrow;
    }

    public static Projectile Arrow(Player.IPlayer shooter, ICreature target)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Arrow.tscn");
        var start = shooter.Coordinate.ToPosition() + INITIAL_POSITION.GetValueOrDefault(shooter.Direction);
        Vector2 end = target.OffsetBodyPosition + target.BodyOffsetTexture.OriginalSize / 2;
        var distance = target.Coordinate.Distance(shooter.Coordinate);
        var arrow = scene.Instantiate<Projectile>();
        var arrowTexture = ArrowAnimation.Instance.OffsetTexture(Direction.RIGHT);
        arrow.Texture = arrowTexture.Texture;
        arrow.Init(distance, start, end);
        return arrow;
    }
}