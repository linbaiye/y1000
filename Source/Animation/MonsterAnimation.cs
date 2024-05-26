using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class MonsterAnimation : AbstractCreatureAnimation<MonsterAnimation>
{
    public static readonly MonsterAnimation Instance = new();
    
    private MonsterAnimation()
    {
        
    }

    private static MonsterAnimation Load(string monsterName, string atdName)
    {
        var atdReader = AtdReader.LoadMonster(monsterName, atdName);
        var spriteReader = AtzSprite.LoadOffsetMonsterSprites(monsterName);
        return new MonsterAnimation()
            .ConfigureState(CreatureState.IDLE, atdReader, spriteReader)
            .ConfigureState(CreatureState.WALK, atdReader, spriteReader)
            .ConfigureState(CreatureState.HURT, atdReader, spriteReader)
            .ConfigureState(CreatureState.ATTACK, atdReader, spriteReader)
            .ConfigureState(CreatureState.FROZEN, atdReader, spriteReader)
            .ConfigureState(CreatureState.DIE, atdReader, spriteReader);
    }
    
    public static MonsterAnimation LoadFor(string name)
    {
        switch (name)
        {
            case "牛":
                return Load("buffalo", "3.atd");
        }
        return Instance;
    }
}