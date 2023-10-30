using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.entity;
using y1000.code.player;

namespace y1000.code.creatures
{
    public interface ICreature : IEntity
    {
        PositionedTexture BodyTexture { get; }

        State State { get; }

        Direction Direction { get; }

        void Move(Direction direction);

        void Attack();

        void Hurt();

        void Die();

    }
}