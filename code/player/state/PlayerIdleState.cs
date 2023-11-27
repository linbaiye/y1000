using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.player.skill;

namespace y1000.code.player.state
{
    public class PlayerIdleState : AbstractCreatureIdleState, IPlayerState
    {

        private static readonly Dictionary<Direction, int> BODY_SPRITE_OFFSET = new ()
        {
            { Direction.UP, 48},
			{ Direction.UP_RIGHT, 51},
			{ Direction.RIGHT, 54},
			{ Direction.DOWN_RIGHT, 57},
			{ Direction.DOWN, 60},
			{ Direction.DOWN_LEFT, 63},
			{ Direction.LEFT, 66},
			{ Direction.UP_LEFT, 69},
        };

        public PlayerIdleState(Player player, Direction direction) : base(player, direction, BODY_SPRITE_OFFSET, 3, 0.5f, PlayerStateFactory.INSTANCE)
        {
        }


        public PlayerIdleState(Player player, Direction direction, AbstractCreatureStateFactory stateFactory) : base(player, direction, BODY_SPRITE_OFFSET, 3, 0.5f, stateFactory)
        {
        }

        protected override SpriteContainer SpriteContainer => ((Player)Creature).IsMale() ? SpriteContainer.LoadMalePlayerSprites("N02"): SpriteContainer.EmptyContainer;
        

        private OffsetTexture GetOffsetTexture(SpriteContainer spriteContainer, int animationSpriteNumber)
        {
            return spriteContainer.Get(BODY_SPRITE_OFFSET.GetValueOrDefault(Direction, -1) + animationSpriteNumber);
        }

        private OffsetTexture GetOffsetTexture(int animationSpriteNumber, IEquipment equipment)
        {
            var path = equipment.SpriteBasePath +  "0";
            return GetOffsetTexture(SpriteContainer.LoadSprites(path), animationSpriteNumber);
        }

        public OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor)
        {
            return GetOffsetTexture(animationSpriteNumber, armor);
        }

        public OffsetTexture HatTexture(int animationSpriteNumber, Hat hat)
        {
            return GetOffsetTexture(animationSpriteNumber, hat);
        }

        public OffsetTexture TrousersTexture(int animationSpriteNumber, Trousers trousers)
        {
            return GetOffsetTexture(animationSpriteNumber, trousers);
        }


        private void OnHurtFinished()
        {
            StopAndChangeState(this);
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
            return true;
        }

        public OffsetTexture WeaponTexture(int animationSpriteNumber, IWeapon weapon)
        {
            SpriteContainer container = SpriteContainer.LoadSprites(weapon.SpriteBasePath + "0" , weapon.Offset);
            return GetOffsetTexture(container, animationSpriteNumber);
        }

    }
}