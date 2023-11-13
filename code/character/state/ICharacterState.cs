using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.player.state;
using y1000.code.world;

namespace y1000.code.character.state
{
    public interface ICharacterState: ICreatureState, IPlayerState
    {
        void OnMouseRightClick(Direction clickDirection) {}

        void OnMouseMotion(Direction direction) {}

        void OnMouseLeftDoubleClick(bool ctrlPressed, bool shiftPressed, ICreature target) { }

        void OnMouseRightReleased() { }
    }
}