using y1000.code;
using y1000.Source.Entity.Animation;

namespace y1000.Source.Creature.Monster;

public class MonsterAnimation : ICreatureAnimation
{
    private MonsterAnimation()
    {
        
    }

    public static readonly MonsterAnimation Instance = new MonsterAnimation();
    public OffsetTexture OffsetTexture(CreatureState state, Direction direction, int millis)
    {
        throw new System.NotImplementedException();
    }

    public int AnimationMillis(CreatureState state)
    {
        throw new System.NotImplementedException();
    }

    public static MonsterAnimation LoadFor(string name)
    {
        switch (name)
        {
            case "牛":
                return ;
        }
        return Instance;
    }
}