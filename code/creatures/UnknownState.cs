using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.player;

namespace y1000.code.creatures
{
    public sealed class UnknownState : ICreatureState
    {

        public Direction Direction => throw new NotImplementedException();

        public State State => throw new NotImplementedException();

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public void Die()
        {
            throw new NotImplementedException();
        }

        public void Hurt()
        {
            throw new NotImplementedException();
        }

        public void Move(Direction direction)
        {
            throw new NotImplementedException();
        }

        public OffsetTexture OffsetTexture(int animationSpriteNumber)
        {
            throw new NotImplementedException();
        }

        public void OnAnimationFinised()
        {
            throw new NotImplementedException();
        }

        public void PlayAnimation()
        {
            throw new NotImplementedException();
        }

        public void Turn(Direction newDirection)
        {
            throw new NotImplementedException();
        }
    }
}