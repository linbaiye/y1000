using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.character.state.snapshot;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.player;
using y1000.code.player.state;
using y1000.code.util;
using y1000.code.world;
using y1000.Source.Creature;
using y1000.Source.Input;
using y1000.Source.Sprite;

namespace y1000.code.character.state
{
    public class CharacterWalkState : AbstractCharacterMoveState
    {
        public override CreatureState State => CreatureState.WALK;


        public CharacterWalkState(OldCharacter character, Direction direction) : base(character, direction, PlayerWalkState.SPRITE_OFFSET,
        PlayerWalkState.SPRITE_NUMBER, PlayerWalkState.STEP, CharacterStateFactory.INSTANCE)
        {

        }


        public CharacterWalkState(OldCharacter character, MouseRightClick rightClick) : base(character, rightClick, PlayerWalkState.SPRITE_OFFSET,
        PlayerWalkState.SPRITE_NUMBER, PlayerWalkState.STEP, CharacterStateFactory.INSTANCE)
        {

        }



        protected override AbstractCreatureState NextState()
        {
            return StateFactory.CreateIdleState(Creature);
        }

        public override OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor)
        {
            return SpriteReader.LoadSprites(armor.SpriteBasePath + "0").Get(SpriteOffset + animationSpriteNumber);
        }

        public override OffsetTexture HatTexture(int animationSpriteNumber, Hat hat)
        {
            return SpriteReader.LoadSprites(hat.SpriteBasePath + "0").Get(SpriteOffset + animationSpriteNumber);
        }

        public override OffsetTexture TrousersTexture(int animationSpriteNumber, Trousers trousers)
        {
            return SpriteReader.LoadSprites(trousers.SpriteBasePath + "0").Get(SpriteOffset + animationSpriteNumber);
        }


        public override OffsetTexture WeaponTexture(int animationSpriteNumber, IWeapon weapon)
        {
            return SpriteReader.LoadSprites(weapon.SpriteBasePath + "0", weapon.Offset).Get(SpriteOffset + animationSpriteNumber);
        }


        protected override SpriteReader SpriteReader => ((Player)Creature).IsMale() ? SpriteReader.LoadOffsetMalePlayerSprites("N02") : SpriteReader.EmptyReader;



        protected override AbstractCharacterMoveState CreateMoveState(MouseRightClick rightClick)
        {
            return new CharacterWalkState(Character, rightClick);
        }

    }
}