using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.player.skill;
using y1000.Source.Sprite;

namespace y1000.code.player.state
{
    public abstract class AbstractPlayerHurtState : AbstractCreatureState, IPlayerState
    {
        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
        {
            { Direction.UP, 184},
            { Direction.UP_RIGHT, 188},
            { Direction.RIGHT, 192},
            { Direction.DOWN_RIGHT, 196},
            { Direction.DOWN, 200},
            { Direction.DOWN_LEFT, 204},
            { Direction.LEFT, 208},
            { Direction.UP_LEFT, 212},
        };

        private readonly Action callback;

        protected AbstractPlayerHurtState(AbstractCreature creature, Direction direction, Action callback) : base(creature, direction)
        {
            creature.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(4, 0.075f, Godot.Animation.LoopModeEnum.None));
            this.callback = callback;
        }

        public override CreatureState State => CreatureState.HURT;

        protected override SpriteReader SpriteReader => ((Player)Creature).IsMale() ? SpriteReader.LoadOffsetMalePlayerSprites("N02") : SpriteReader.EmptyReader;

        protected override int SpriteOffset => SPRITE_OFFSET.GetValueOrDefault(Direction, -1);


        private string GetEquipmentSpritePath(IEquipment equipment)
        {
            return equipment.SpriteBasePath + "0";
        }

        private OffsetTexture GetOffsetTexture(int animationSpriteNumber, IEquipment equipment)
        {
            return SpriteReader.LoadSprites(GetEquipmentSpritePath(equipment)).Get(SpriteOffset + animationSpriteNumber);
        }

        public OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor)
        {
            return GetOffsetTexture(animationSpriteNumber, armor);
        }

        protected void InvokeCallback()
        {
            callback.Invoke();
        }

        public OffsetTexture HatTexture(int animationSpriteNumber, Hat hat)
        {
            return GetOffsetTexture(animationSpriteNumber, hat);
        }

        public OffsetTexture TrousersTexture(int animationSpriteNumber, Trousers trousers)
        {
            return GetOffsetTexture(animationSpriteNumber, trousers);
        }

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