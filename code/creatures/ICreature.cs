using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.entity;
using y1000.code.player;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Entity;

namespace y1000.code.creatures
{
    public interface ICreature : IEntity
    {
        OffsetTexture BodyTexture { get; }

        Direction Direction { get; }

        void Move(Direction direction);

        void Turn(Direction direction);

        void Attack();

        void Hurt();

        void Die();

        void Remove();

        Direction DirectionTo(ICreature another);

        Point Coordinate { get; }

        Rect2I HoverRect();
    }
}