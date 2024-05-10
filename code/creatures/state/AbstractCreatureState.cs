using y1000.code.player;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Sprite;

namespace y1000.code.creatures.state
{
    public abstract class AbstractCreatureState : ICreatureState
    {
        private Direction direction;

        private readonly AbstractCreature creature;

        private readonly AbstractCreatureStateFactory stateFactory;

        protected AbstractCreatureState(AbstractCreature creature, Direction direction)
        {
            this.creature = creature;
            this.direction = direction;
            stateFactory = new SimpleCreatureStateFactory();
        }

        protected AbstractCreatureState(AbstractCreature creature, Direction direction, AbstractCreatureStateFactory _stateFactory)
        {
            this.creature = creature;
            this.direction = direction;
            stateFactory = _stateFactory;
        }

        protected AbstractCreature Creature => creature;

        public Direction Direction => direction;

        public abstract CreatureState State { get; }

        protected AbstractCreatureStateFactory StateFactory => stateFactory;

        public virtual void Move(Direction direction)
        {

        }

        public virtual void PlayAnimation()
        {
            Creature.AnimationPlayer.Play(State + "/" + Direction);
        }

        protected void StopAndChangeState(AbstractCreatureState newState)
        {
            creature.AnimationPlayer.Stop();
            creature.ChangeState(newState);
        }

        protected void SetDirection(Direction newDirection)
        {
            direction = newDirection;
        }


        public virtual void Turn(Direction newDirection) {}

        public virtual void OnAnimationFinised() {}

        public virtual void Attack() { }

        public virtual void Hurt() { }

        public virtual void Die()
        {
            if (CreatureState.DIE != State)
                StopAndChangeState(StateFactory.CreateDieState(Creature));
        }

        protected abstract int SpriteOffset { get; }

        protected abstract SpriteReader SpriteReader { get; }

        public virtual OffsetTexture OffsetTexture(int animationSpriteNumber)
        {
            return SpriteReader.Get(SpriteOffset + animationSpriteNumber);
        }
    }
}