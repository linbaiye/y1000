using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.player;

namespace y1000.code.creatures
{
    public interface ICreatureState
    {
        Direction Direction { get; }

        State State { get; }

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