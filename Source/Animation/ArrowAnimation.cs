using System.Collections.Generic;
using y1000.Source.Creature;

namespace y1000.Source.Animation;

public sealed class ArrowAnimation
{
    public static readonly ArrowAnimation Instance = Load();

    private static readonly Dictionary<Direction, int> OFFSET_MAP = new()
    {
        { Direction.UP, 0 },
        { Direction.UP_RIGHT, 10 },
        { Direction.RIGHT, 20 },
        { Direction.DOWN_RIGHT, 30 },
        { Direction.DOWN, 40 },
        { Direction.DOWN_LEFT, 50 },
        { Direction.LEFT, 60 },
        { Direction.UP_LEFT, 70 },
    };

    private ArrowAnimation(SpriteReader reader)
    {
        Reader = reader;
    }
    
    private SpriteReader Reader { get; }
    
    public OffsetTexture OffsetTexture(Direction direction)
    {
        return Reader.Get(OFFSET_MAP.GetValueOrDefault(direction, 0));
    }

    private static ArrowAnimation Load()
    {
        return new (SpriteReader.LoadEffect("Arrow"));
    }
}