using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.player;

namespace y1000.code.creatures
{
    public interface ICreature
    {
        PositionedTexture BodyTexture { get; }

        State State { get; }

        Direction Direction { get; }

        void ChangeState(IPlayerState newState);
    }
}