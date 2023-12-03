using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.character.state.snapshot;
using y1000.code.creatures;
using y1000.code.player.state;
using y1000.code.world;

namespace y1000.code.character.state
{
    public interface ICharacterState: IPlayerState
    {
        void OnMouseRightClick(Direction clickDirection) {}

        void OnMouseMotion(Direction direction) {}

        void OnMouseLeftDoubleClick(bool ctrlPressed, bool shiftPressed, ICreature target) { }

        void OnMouseRightReleased() { }

        void OnMouseMotion(Character character, Direction direction) {}

        void OnMouseRightClick(Character character, Direction clickDirection) {}

        void OnMouseRightReleased(Character character) { }

        IStateSnapshot TakeSnapshot(Character character) { return PositionSnapshot.ForState(this, character); }
    }
}