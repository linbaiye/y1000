using Godot;
using NLog;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Util;

namespace y1000.Source.Entity;

public partial class Projectile : Sprite2D
{
    private static readonly ILogger LOG = LogManager.GetCurrentClassLogger();

    private Vector2 vect;

    private float lengthSeconds;

    private float elapsed = 0;
    
    private void Init(Player.IPlayer player, ICreature target)
    {
        var tpos = target.OffsetPosition;
        var size = target.BodyOffsetTexture.Texture.GetSize();
        var distance = target.Coordinate.Distance(player.Coordinate);
        lengthSeconds = distance * 0.05f;
        vect = ((tpos +  (size / 2)) - player.Coordinate.ToPosition()) / lengthSeconds;
        //vect = (target.Coordinate - player.Coordinate).ToPosition() + (VectorUtil.TileSize / 2);
        Position = player.Position;
        //Position = player.Coordinate.Move(player.Direction).ToPosition();
        var arrowTexture = ArrowAnimation.Instance.OffsetTexture(player.Direction);
        Offset = arrowTexture.Offset;
        Texture = arrowTexture.Texture;
    }

    public override void _Process(double delta)
    {
        elapsed += (float)delta;
        Position += (vect * (float)delta);
        if (elapsed >= lengthSeconds)
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