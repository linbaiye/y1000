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

        int GetSpriteOffset();

        void Move(Direction direction);

        void Turn(Direction newDirection);

        void OnAnimationFinised();

        void Attack();

        void Hurt();

        void PlayAnimation();

        void Die();
    }
}