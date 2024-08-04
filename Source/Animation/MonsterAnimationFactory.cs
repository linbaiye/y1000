using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class MonsterAnimationFactory
{
    public static readonly MonsterAnimationFactory Instance = new();

    private readonly ISpriteRepository _spriteRepository;
    private readonly IAtdRepository _atdRepository;
    private MonsterAnimationFactory()
    {
        _spriteRepository = FilesystemSpriteRepository.Instance;
        _atdRepository = FilesystemAtdRepository.Instance;
    }

    public MonsterAnimation Load(string atz, string atd)
    {
        var sprite = _spriteRepository.LoadByNumber(atz);
        var atdStructure = _atdRepository.LoadByName(atd);
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

    public MonsterAnimation? LoadEffect(string atz, string atd)
    {
        if (!_spriteRepository.Exists(atz + "m"))
        {
            return null;
        }
        return Load(atz + "m", atd);
    }
}