using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
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

        public override OffsetTexture ChestTexture(int animationSpriteNumber)
        {
            throw new NotImplementedException();
        }


        protected override SpriteContainer SpriteContainer => ((Player)Creature).IsMale() ? SpriteContainer.LoadMalePlayerSprites("N02") : SpriteContainer.EmptyContainer;
    }
}