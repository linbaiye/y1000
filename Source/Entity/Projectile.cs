using System.Collections.Generic;
using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Util;

namespace y1000.Source.Entity;

public partial class Projectile : Sprite2D
{
    private static readonly ILogger LOG = LogManager.GetCurrentClassLogger();

    private Vector2 _vect;

    private float _lengthSeconds;

    private float _elapsed = 0;
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
    
    private void Init(Player.IPlayer player, ICreature target)
    {
        Position = player.Coordinate.ToPosition() + INITIAL_POSITION.GetValueOrDefault(player.Direction);
        var distance = target.Coordinate.Distance(player.Coordinate);
        Vector2 centerPoint = target.OffsetBodyPosition + target.BodyOffsetTexture.OriginalSize / 2;
        _lengthSeconds = distance * 0.03f;
        _vect = centerPoint - Position;
        Rotation = _vect.Normalized().Angle();
        _vect /= _lengthSeconds;
        var arrowTexture = ArrowAnimation.Instance.OffsetTexture(Direction.RIGHT);
        Texture = arrowTexture.Texture;
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
    

    public static Projectile Arrow(Player.IPlayer shooter, ICreature target)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://Scenes/Arrow.tscn");
        var arrow = scene.Instantiate<Projectile>();
        arrow.Init(shooter, target);
        return arrow;
    }
}