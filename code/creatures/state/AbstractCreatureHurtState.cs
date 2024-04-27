using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.Source.Creature;

namespace y1000.code.creatures.state
{
    public abstract class AbstractCreatureHurtState : AbstractCreatureState
    {

        public AbstractCreatureHurtState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
            creature.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(3, 0.07f, Animation.LoopModeEnum.None));
        }

        public override CreatureState State => CreatureState.HURT;


        public override void OnAnimationFinised()
        {
            TextureProgressBar hpbar = Creature.GetNode<TextureProgressBar>("HPBar");
            hpbar.Value = Math.Max(0, hpbar.Value - 10);
            if (hpbar.Value <= 0)
            {
                Creature.ChangeState(StateFactory.CreateDieState(Creature));
            }
            else
            {
                Creature.ChangeState(StateFactory.CreateIdleState(Creature));
            }
        }
    }
}