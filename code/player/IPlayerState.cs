using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;

namespace y1000.code.player
{
    public interface IPlayerState : ICreatureState
    {
        void OnAnimationFinished(StringName animationName);

        void RightMousePressed(Vector2 mousePosition);

        void RightMouseRleased();

        void Sit();


        void Attack(ICreature target) {}
    }
}