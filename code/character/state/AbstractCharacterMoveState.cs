using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.character.state.input;
using y1000.code.character.state.snapshot;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.networking.message;
using y1000.code.player;
using y1000.code.player.skill;
using y1000.code.player.state;
using y1000.code.util;

namespace y1000.code.character.state
{
    public abstract class AbstractCharacterMoveState : AbstractCreatureMoveState, ICharacterState
    {

        private bool keepWalking;

        private Direction nextDirection;

        private double pausedPosition = 0;

        private IInput? lastReceivedInput;

        protected AbstractCharacterMoveState(Character character, Direction direction,
         Dictionary<Direction, int> spriteOffset, int spriteNumber, float step, AbstractCreatureStateFactory stateFactory) :
          base(character, direction, spriteOffset, spriteNumber, step, stateFactory, ComputeSpeed(character))
        {
            keepWalking = true;
            nextDirection = Direction;
        }




        private static float ComputeSpeed(Character character)
        {
            return character.Bufa != null ? character.Bufa.Speed : 1.0f;
        }

        public void OnMouseMotion( Direction direction)
        {
            nextDirection = direction;
        }
        
        public void OnMouseRightClick( Direction clickDirection)
        {
            keepWalking = true;
            nextDirection = clickDirection;
        }

        protected abstract AbstractCreatureState NextState();

        public override void OnAnimationFinised()
        {
            UpdateCooridnate();
            if (lastReceivedInput == null) 
            {

            }
            if (keepWalking)
            {
                var next = Creature.Coordinate.Next(nextDirection);
                if (((Character)Creature).CanMove(next))
                {
                    ChangeSpeed(ComputeSpeed((Character)Creature));
                    ReadyToMoveTo(nextDirection, next);
                    PlayAnimation();
                    return;
                }
            } else {
                Creature.AnimationPlayer.SpeedScale = 1.0f;
                StopAndChangeState(NextState());
            }
        }

        public void OnMouseRightReleased()
        {
            keepWalking = false;
        }

        public abstract OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor);

        public abstract OffsetTexture HatTexture(int animationSpriteNumber, Hat hat);

        public abstract OffsetTexture TrousersTexture(int animationSpriteNumber, Trousers trousers);

        private void OnHurtDone()
        {
            Creature.ChangeState(this);
            Creature.AnimationPlayer.Play(State + "/" + Direction);
            Creature.AnimationPlayer.Advance(pausedPosition);
        }

        public override void Hurt()
        {
            Creature.AnimationPlayer.Pause();
            pausedPosition = Creature.AnimationPlayer.CurrentAnimationPosition;
            Creature.ChangeState(new PlayerMoveHurtState(Creature, Direction, OnHurtDone));
        }

        public void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            lastReceivedInput = rightClick;
        }


        public void OnMouseRightReleased(Character character, MouseRightRelease rightRelease)
        {
            lastReceivedInput = rightRelease;
        }


        public bool PressBufa(IBufa bufa)
        {
            return true;
        }

        public abstract OffsetTexture WeaponTexture(int animationSpriteNumber, IWeapon weapon);

    }
}