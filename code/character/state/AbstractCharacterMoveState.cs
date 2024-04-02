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
using y1000.code.networking.message;
using y1000.code.player;
using y1000.code.player.skill;
using y1000.code.player.state;
using y1000.code.util;
using y1000.Source.Input;

namespace y1000.code.character.state
{
    public abstract class AbstractCharacterMoveState : AbstractCreatureMoveState, IOldCharacterState
    {

        private bool keepWalking;

        private Direction nextDirection;

        private double pausedPosition = 0;

        private IInput? lastReceivedInput = null;

        private MouseRightClick current;


        protected AbstractCharacterMoveState(OldCharacter character, Direction direction,
         Dictionary<Direction, int> spriteOffset, int spriteNumber, float step, AbstractCreatureStateFactory stateFactory) :
          base(character, direction, spriteOffset, spriteNumber, step, stateFactory, ComputeSpeed(character))
        {
            keepWalking = true;
            nextDirection = Direction;
        }

         protected AbstractCharacterMoveState(OldCharacter character, MouseRightClick current,
         Dictionary<Direction, int> spriteOffset, int spriteNumber, float step, AbstractCreatureStateFactory stateFactory) :
          base(character, current.Direction, spriteOffset, spriteNumber, step, stateFactory, ComputeSpeed(character))
        {
            keepWalking = true;
            nextDirection = Direction;
            this.current = current;
        }



        protected OldCharacter Character => (OldCharacter)Creature;


        private static float ComputeSpeed(OldCharacter character)
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


        protected abstract AbstractCharacterMoveState CreateMoveState(MouseRightClick rightClick);

        protected abstract AbstractCreatureState NextState();

        private void ContinueMoving(MouseRightClick rightClick)
        {
            Character.SendActAndSavePredict(rightClick, () =>
            {
                var next = Creature.Coordinate.Next(rightClick.Direction);
                if (Character.CanMove(next))
                {
                    LOG.Debug("Changing to move .");
                    StopAndChangeState(CreateMoveState(rightClick));
                }
                else
                {
                    StopAndChangeState(NextState());
                }
            });

        }

        public override void OnAnimationFinised()
        {
            UpdateCooridnate();
            LOG.Debug("Last input :" + lastReceivedInput);
            if (lastReceivedInput == null)
            {
                StopAndChangeState(NextState());
                //MouseRightClick nextInput = InputFactory.CreateMouseMoveInput(current.Direction);
                //ContinueMoving(nextInput);
            }
            else if (lastReceivedInput is MouseRightClick rightClick)
            {
                ContinueMoving(rightClick);
            }
            else if (lastReceivedInput is MouseRightRelease rightRelease)
            {
                Character.SendActAndSavePredict(rightRelease, () =>
                {
                    StopAndChangeState(NextState());
                });
            }
            else if (lastReceivedInput is RightMousePressedMotion rightMousePressedMotion)
            {
                Character.SendActAndSavePredict(rightMousePressedMotion, () =>
                {
                    ContinueMoving(InputFactory.CreateMouseMoveInput(rightMousePressedMotion.Direction));
                });
            }
            /*if (keepWalking)
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
            }*/
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

        public void OnMouseRightClicked(OldCharacter character, MouseRightClick rightClick)
        {
            lastReceivedInput = rightClick;
        }


        public void OnMouseRightReleased(OldCharacter character, MouseRightRelease rightRelease)
        {
            lastReceivedInput = rightRelease;
        }

        public void OnMouseRightReleased(OldCharacter character, RightMousePressedMotion rightMousePressedMotion)
        {
            lastReceivedInput = rightMousePressedMotion;
        }



        public bool PressBufa(IBufa bufa)
        {
            return true;
        }


        public IStateSnapshot Snapshot(OldCharacter character)
        {
            Point nextCoor = character.Coordinate.Next(current.Direction);
            return new PostionDirectionSnapshot()
            {
                Direction = Direction,
                Coordinate = nextCoor
            };
        }

        public abstract OffsetTexture WeaponTexture(int animationSpriteNumber, IWeapon weapon);

    }
}