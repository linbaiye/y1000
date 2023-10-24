using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public interface IPlayerState
    {
        State State { get; }

        Direction Direction { get; }

        void PhysicsProcess(double delta);

        void OnAnimationFinished(StringName animationName);

        void RightMousePressed(Vector2 mousePosition);

        void RightMouseRleased();

        void Attack();

        PositionedTexture BodyTexture { get; }
    }
}