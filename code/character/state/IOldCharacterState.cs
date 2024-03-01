using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.character.state.input;
using y1000.code.character.state.Prediction;
using y1000.code.character.state.snapshot;
using y1000.code.creatures;
using y1000.code.player.state;
using y1000.code.world;

namespace y1000.code.character.state
{
    public interface IOldCharacterState: IPlayerState
    {
        void OnMouseRightClick(Direction clickDirection) {}

        void OnMouseMotion(Direction direction) {}

        void OnMouseLeftDoubleClick(bool ctrlPressed, bool shiftPressed, ICreature target) { }

        void OnMouseRightReleased() { }

        void OnMouseMotion(OldCharacter character, Direction direction) {}

        void OnMouseMotion(OldCharacter character, RightMousePressedMotion mousePressedMotion) {}

        void OnMouseRightClicked(OldCharacter character, MouseRightClick mouseRightClick) {}

        void OnMouseRightReleased(OldCharacter character, MouseRightRelease mouseRightRelease) { }

        IStateSnapshot Snapshot(OldCharacter character) { return PositionSnapshot.ForState(this, character); }
    }
}