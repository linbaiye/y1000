using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.character;
using y1000.code.character.state;
using y1000.code.entity.equipment;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.Source.Creature;
using y1000.Source.Entity.Animation;
using y1000.Source.Sprite;
using AbstractCreature = y1000.code.creatures.AbstractCreature;

namespace y1000.code.player.state
{
    public class PlayerGetupState : AbstractPlayerSitState
    {
        private bool acceptHurt;
        private double pausedPosition;
        public PlayerGetupState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
            acceptHurt = true;
        }

        public override void PlayAnimation()
        {
            Creature.AnimationPlayer.Play(State + "/" + Direction);
        }

        public override void OnAnimationFinised()
        {
            StopAndChangeState(new CharacterIdleState((OldCharacter)Creature, Direction));
        }

        private void OnHurtDone()
        {
            Creature.ChangeState(this);
            Creature.AnimationPlayer.Play(State + "/" + Direction);
            Creature.AnimationPlayer.Advance(pausedPosition);
        }

        public override void Hurt()
        {
            if (acceptHurt)
            {
                Creature.AnimationPlayer.Pause();
                pausedPosition = Creature.AnimationPlayer.CurrentAnimationPosition;
                Creature.ChangeState(new PlayerMoveHurtState(Creature, Direction, OnHurtDone));
                acceptHurt = false;
            }
        }


        private int ComputeAnimationSpriteNumber(int animationSpriteNumber)
        {
            return SpriteOffset + total - animationSpriteNumber - 1;
        }

        public override OffsetTexture OffsetTexture(int animationSpriteNumber)
        {
            return SpriteReader.Get(ComputeAnimationSpriteNumber(animationSpriteNumber)) ;
        }

        protected override OffsetTexture GetTexture(int animationSpriteNumber, IEquipment equipment)
        {
            return SpriteReader.LoadSprites(equipment.SpriteBasePath + "0").Get(ComputeAnimationSpriteNumber(animationSpriteNumber));
        }
    }
}