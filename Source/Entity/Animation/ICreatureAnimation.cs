using y1000.code;
using y1000.Source.Creature;

namespace y1000.Source.Entity.Animation;

public interface ICreatureAnimation
{
    OffsetTexture OffsetTexture(CreatureState state, Direction direction, int millis);

    int AnimationMillis(CreatureState state);

}