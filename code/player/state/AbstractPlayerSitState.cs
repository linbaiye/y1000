using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character;
using y1000.code.creatures.state;
using y1000.code.entity.equipment;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.player.skill;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Sprite;
using AbstractCreature = y1000.code.creatures.AbstractCreature;

namespace y1000.code.player.state
{
    public abstract class AbstractPlayerSitState: AbstractCreatureState, IPlayerState
    {
        private static readonly Dictionary<Direction, int> BODY_SPRITE_OFFSET = new ()
        {
            { Direction.UP, 144},
			{ Direction.UP_RIGHT, 149},
			{ Direction.RIGHT, 154},
			{ Direction.DOWN_RIGHT, 159},
			{ Direction.DOWN, 164},
			{ Direction.DOWN_LEFT, 169},
			{ Direction.LEFT, 174},
			{ Direction.UP_LEFT, 179},
        };

        protected const int total = 5;

        private const float step = 0.12f;

        protected const float animationLength = total * step;


        public AbstractPlayerSitState(AbstractCreature creature, Direction direction) : base(creature, direction, PlayerStateFactory.INSTANCE)
        {
            creature.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(total, step));
        }

        public override CreatureState State => CreatureState.SIT;

        protected override int SpriteOffset => BODY_SPRITE_OFFSET.GetValueOrDefault(Direction, -1);

        protected override SpriteReader SpriteReader => ((IPlayer)Creature).IsMale() ?  SpriteReader.LoadOffsetMalePlayerSprites("N02"): SpriteReader.EmptyReader;

        protected abstract OffsetTexture GetTexture(int animationSpriteNumber, IEquipment equipment);

        public OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor)
        {
            return GetTexture(animationSpriteNumber, armor);
        }

        public OffsetTexture HatTexture(int animationSpriteNumber, Hat hat)
        {
            return GetTexture(animationSpriteNumber, hat);
        }

        public OffsetTexture TrousersTexture(int animationSpriteNumber, Trousers trousers)
        {
            return GetTexture(animationSpriteNumber, trousers);
        }

        public virtual void Sit() {}

        public bool PressBufa(IBufa bufa)
        {
            StopAndChangeState(new PlayerIdleState((OldCharacter)Creature, Direction));
            return true;
        }

        public OffsetTexture WeaponTexture(int animationSpriteNumber, IWeapon weapon)
        {
            throw new NotImplementedException();
        }
    }
}