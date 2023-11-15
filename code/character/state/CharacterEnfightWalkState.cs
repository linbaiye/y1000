using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.player;

namespace y1000.code.character.state
{
    public sealed class CharacterEnfightWalkState : AbstractCharacterMoveState
    {
        private readonly ICreature? target;

        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
        {
            { Direction.UP, 72},
            { Direction.UP_RIGHT, 78},
            { Direction.RIGHT, 84},
            { Direction.DOWN_RIGHT, 90},
            { Direction.DOWN, 96},
            { Direction.DOWN_LEFT, 102},
            { Direction.LEFT, 108},
            { Direction.UP_LEFT, 114},
        };

        public CharacterEnfightWalkState(Character character, Direction direction, ICreature? target) :
         base(character, direction, SPRITE_OFFSET, 6, 0.15f, CharacterStateFactory.INSTANCE)
        {
            this.target = target;
        }

        public override State State => State.ENFIGHT_WALK;

        protected override SpriteContainer SpriteContainer => ((Character)Creature).IsMale() ? SpriteContainer.LoadMalePlayerSprites("N02") : SpriteContainer.EmptyContainer;

        protected override AbstractCreatureState NextState()
        {
            return new CharacterEnfightState((Character)Creature, Direction, target);
        }

        public override OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor)
        {
            return SpriteContainer.LoadSprites(armor.SpriteBasePath + "0").Get(SPRITE_OFFSET.GetValueOrDefault(Direction, -1)+ animationSpriteNumber);
        }

        public override OffsetTexture HatTexture(int animationSpriteNumber, Hat hat)
        {
            return SpriteContainer.LoadSprites(hat.SpriteBasePath + "0").Get(SPRITE_OFFSET.GetValueOrDefault(Direction, -1)+ animationSpriteNumber);
        }

    }
}