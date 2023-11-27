using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.player;
using y1000.code.player.state;
using y1000.code.util;
using y1000.code.world;

namespace y1000.code.character.state
{
    public class CharacterWalkState : AbstractCharacterMoveState
    {
        public override State State => State.WALK;


        public CharacterWalkState(Character character, Direction direction) : base(character, direction, PlayerWalkState.SPRITE_OFFSET,
        PlayerWalkState.SPRITE_NUMBER, PlayerWalkState.STEP, CharacterStateFactory.INSTANCE)
        {

        }

        protected override AbstractCreatureState NextState()
        {
            return StateFactory.CreateIdleState(Creature);
        }

        public override OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor)
        {
            return SpriteContainer.LoadSprites(armor.SpriteBasePath + "0").Get(SpriteOffset + animationSpriteNumber);
        }

        public override OffsetTexture HatTexture(int animationSpriteNumber, Hat hat)
        {
            return SpriteContainer.LoadSprites(hat.SpriteBasePath + "0").Get(SpriteOffset + animationSpriteNumber);
        }

        public override OffsetTexture TrousersTexture(int animationSpriteNumber, Trousers trousers)
        {
            return SpriteContainer.LoadSprites(trousers.SpriteBasePath + "0").Get(SpriteOffset + animationSpriteNumber);
        }


        public override OffsetTexture WeaponTexture(int animationSpriteNumber, IWeapon weapon)
        {
            return SpriteContainer.LoadSprites(weapon.SpriteBasePath + "0", weapon.Offset).Get(SpriteOffset + animationSpriteNumber);
        }


        protected override SpriteContainer SpriteContainer => ((Player)Creature).IsMale() ? SpriteContainer.LoadMalePlayerSprites("N02") : SpriteContainer.EmptyContainer;


    }
}