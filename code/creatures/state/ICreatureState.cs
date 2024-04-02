using y1000.code.player;

namespace y1000.code.creatures.state
{
    public interface ICreatureState
    {
        Direction Direction { get; }

        CreatureState State { get; }

        void Move(Direction direction);

        void Turn(Direction newDirection);

        void OnAnimationFinised();

        void Attack();

        void Attack(Direction direction) { }

        void Hurt();

        void PlayAnimation();

        void Die();

        OffsetTexture OffsetTexture(int animationSpriteNumber);

        void Process(double delta) { }
    }
}