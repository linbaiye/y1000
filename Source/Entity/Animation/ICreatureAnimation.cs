using y1000.code;
using y1000.Source.Creature;

namespace y1000.Source.Entity.Animation;

public interface ICreatureAnimation
{
    OffsetTexture NoneOffsetTexture(CreatureState state, Direction direction, int millis);
}