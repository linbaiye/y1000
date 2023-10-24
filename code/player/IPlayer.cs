using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public interface IPlayer
    {
        PositionedTexture BodyTexture { get; }

        State State { get; }

        Direction Direction { get; }

        AnimationPlayer AnimationPlayer { get; }

        void ChangeState(IPlayerState newState);

    }
}