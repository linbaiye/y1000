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
    
    private void Init(Player.IPlayer player, ICreature target)
    {
        var tpos = target.OffsetBodyPosition;
        var distance = target.Coordinate.Distance(player.Coordinate);
        _lengthSeconds = distance * 0.03f;
        _vect = tpos - player.OffsetBodyPosition;
        //_vect = ((tpos +  (size / 2)) - player.Coordinate.ToPosition()) / _lengthSeconds;
        //vect = (target.Coordinate - player.Coordinate).ToPosition() + (VectorUtil.TileSize / 2);
        //Position = player.BodyPosition;
        Position = player.OffsetBodyPosition;
        Rotation = _vect.Angle();
        _vect /= _lengthSeconds;
        var arrowTexture = ArrowAnimation.Instance.OffsetTexture(Direction.RIGHT);
        var offsetTexture = ArrowAnimation.Instance.OffsetTexture(player.Direction);
        //Rotation = tpos.AngleTo(Position);
        if (Rotation == 0)
        {
            LOG.Debug("Target Id {0}, player Id {1}.", target.Id, player.Id);
        }
        //Rotation = angle;
        //var transform = Transform;
        //var newTrans = new Transform2D(angle, transform.Origin);
        //Transform = newTrans;
        Offset = offsetTexture.Offset + new Vector2(16, -12);
        Texture = arrowTexture.Texture;
    }

    public override void _Process(double delta)
    {
        _elapsed += (float)delta;
        Position += (_vect * (float)delta);
        if (_elapsed >= _lengthSeconds)
        {
            LOG.Debug("Enough time, delete now.");
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