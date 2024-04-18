using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.entity.equipment;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.Source.Sprite;

namespace y1000.code.player.state
{
    public class PlayerSitState : AbstractPlayerSitState
    {

        private bool sat;

        private bool acceptHurt;

        public PlayerSitState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
            sat = false;
            acceptHurt = true;
        }

        public override void Hurt()
        {
            if (sat)
            {
                StopAndChangeState(new PlayerMoveHurtState(Creature, Direction, OnHurtDone));
            }
            else if (acceptHurt)
            {
                StopAndChangeState(new PlayerMoveHurtState(Creature, Direction, OnHurtDone));
                acceptHurt = false;
            }
        }

        public void OnHurtDone()
        {
            Creature.ChangeState(this);
            if (sat)
            {
                PlayAnimation();
                Creature.AnimationPlayer.Advance(animationLength);
            }
            else
            {
                PlayAnimation();
            }

        }

        public override void OnAnimationFinised()
        {
            sat = true;
        }


        public override void Sit()
        {
            if (sat)
            {
                StopAndChangeState(new PlayerGetupState(Creature, Direction));
            }
        }

        protected override OffsetTexture GetTexture(int animationSpriteNumber, IEquipment equipment)
        {
            return SpriteReader.LoadSprites(equipment.SpriteBasePath + "0").Get(SpriteOffset + animationSpriteNumber);
        }
    }
}