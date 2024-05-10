using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.player.skill;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Entity.Animation;
using y1000.Source.Sprite;

namespace y1000.code.player.state
{
    public abstract class AbstractPlayerAttackState : AbstractCreatureState, IPlayerState
    {
        public override CreatureState State => CreatureState.ATTACKING;

        private readonly Dictionary<Direction, int> spriteOffset;

        protected AbstractPlayerAttackState(Player creature,
         Direction direction,
         AbstractCreatureStateFactory _stateFactory,
         Dictionary<Direction, int> so) : base(creature, direction, _stateFactory)
        {
            spriteOffset = so;
        }

        public override void OnAnimationFinised()
        {
            StopAndChangeState(new PlayerEnfightState((Player)Creature, Direction));
        }

        protected override int SpriteOffset => spriteOffset.GetValueOrDefault(Direction, -1);

        protected abstract string GetChestSpritePath(ChestArmor armor);

        protected abstract string GetHatSpritePath(Hat hat);

        protected abstract string GetTrousersPath(Trousers trousers);

        public OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor)
        {
            string path = "armor/" + (armor.IsMale ? "male/": "female/") + "chest/" + GetChestSpritePath(armor);
            return SpriteReader.LoadSprites(path).Get(SpriteOffset + animationSpriteNumber);
        }

        public OffsetTexture HatTexture(int animationSpriteNumber, Hat hat)
        {
            return SpriteReader.LoadSprites(GetHatSpritePath(hat)).Get(SpriteOffset + animationSpriteNumber);
        }

        public OffsetTexture TrousersTexture(int animationSpriteNumber, Trousers trousers)
        {
            return SpriteReader.LoadSprites(GetTrousersPath(trousers)).Get(SpriteOffset + animationSpriteNumber);
        }


        private void OnHurtFinished()
        {
            Creature.ChangeState(this);
            PlayAnimation();
        }

        public override void Hurt()
        {
            StopAndChangeState(new PlayerStandHurtState(Creature, Direction, OnHurtFinished));
        }

        public void Sit()
        {
            StopAndChangeState(new PlayerSitState(Creature, Direction));
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