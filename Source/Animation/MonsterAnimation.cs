using y1000.code;
using y1000.Source.Creature;
using y1000.Source.Creature.Monster;
using y1000.Source.Sprite;

namespace y1000.Source.Animation;

public class MonsterAnimation : AbstractCreatureAnimation<MonsterAnimation>
{
    public static readonly MonsterAnimation Instance = new();
    private readonly MonsterSdbReader _sdbReader;

    private readonly IAtdRepository _atdRepository;

    private readonly ISpriteRepository _spriteRepository;

    public MonsterAnimation()
    {
        _sdbReader = MonsterSdbReader.Instance;
        _atdRepository = FilesystemAtdRepository.Instance;
        _spriteRepository = FilesystemSpriteRepository.Instance;
    }
}