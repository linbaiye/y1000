using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;

namespace y1000.code.player
{
    public interface IPlayer : ICreature
    {
        AnimationPlayer AnimationPlayer { get; }

        void ChangeState(IPlayerState newState);


        void Move(Vector2 mousePosition);

        void StopMove();

        void Attack(ICreature target);
    }
}