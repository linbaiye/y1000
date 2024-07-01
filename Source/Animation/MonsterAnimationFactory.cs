using y1000.Source.Creature;
using y1000.Source.Creature.Monster;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class MonsterAnimationFactory
{
    public static readonly MonsterAnimationFactory Instance = new();

    private readonly ISpriteRepository _spriteRepository;
    private readonly MonsterSdbReader _monsterSdb;
    private readonly IAtdRepository _atdRepository;
    private readonly NpcSdbReader _npcSdbReader;
    private MonsterAnimationFactory()
    {
        _spriteRepository = FilesystemSpriteRepository.Instance;
        _monsterSdb = MonsterSdbReader.Instance;
        _atdRepository = FilesystemAtdRepository.Instance;
        _npcSdbReader = NpcSdbReader.Instance;
    }


    public MonsterAnimation Load(string name)
    {
        var atdName = _monsterSdb.Contains(name) ? _monsterSdb.GetAtdName(name) : _npcSdbReader.GetAtdName(name);
        var spriteName = "z" + (_monsterSdb.Contains(name)
            ? _monsterSdb.GetSpriteName(name)
            : _npcSdbReader.GetSpriteName(name));
        var sprite = _spriteRepository.LoadByNumber(spriteName);
        var atdStructure = _atdRepository.LoadByName(atdName);
        var animation = new MonsterAnimation()
            .ConfigureState(CreatureState.IDLE, atdStructure, sprite)
            .ConfigureState(CreatureState.WALK, atdStructure, sprite)
            .ConfigureState(CreatureState.HURT, atdStructure, sprite)
            .ConfigureState(CreatureState.FROZEN, atdStructure, sprite)
            .ConfigureState(CreatureState.DIE, atdStructure, sprite);
        if (atdStructure.HasState(CreatureState.ATTACK))
        {
            animation.ConfigureState(CreatureState.ATTACK, atdStructure, sprite);
        }
        return animation;
    }
}