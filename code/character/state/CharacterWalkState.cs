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

        private bool keepWalking;
        private Direction nextDirection;

        public override State State => State.WALK;

        protected override SpriteContainer SpriteContainer => throw new NotImplementedException();

        public CharacterWalkState(Player _player, Direction direction) : base(_player, direction, CharacterStateFactory.INSTANCE)
        {
            keepWalking = true;
            nextDirection = Direction;
        }



        protected override AbstractCreatureState NextState()
        {
            return StateFactory.CreateIdleState(Creature);
        }
    }
}