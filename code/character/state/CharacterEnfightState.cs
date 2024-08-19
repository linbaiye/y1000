using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.character.state.snapshot;
using y1000.code.player;
using y1000.code.player.state;
using y1000.code.util;
using y1000.Source.Creature;
using ICreature = y1000.code.creatures.ICreature;

namespace y1000.code.character.state
{
    public class CharacterEnfightState : PlayerEnfightState, IOldCharacterState
    {
        private readonly ICreature? target;

        public CharacterEnfightState(OldCharacter character, Direction direction, ICreature? target) : base(character, direction)
        {
            this.target = target;
        }

        public void OnMouseRightClick(Direction clickDirection)
        {
            ((OldCharacter)Creature).MoveOrTurn(clickDirection);
        }

        public void OnMouseMotion(Direction direction)
        {
            ((OldCharacter)Creature).MoveOrTurn(direction);
        }

        public override void Move(Direction newDirection)
        {
            StopAndChangeState(new CharacterEnfightWalkState((OldCharacter)Creature, newDirection, target));
        }

        public void Process(double delta)
        {
            if (target == null)
            {
                return;
            }
            if (target.Coordinate.IsNextTo(Creature.Coordinate))
            {
                StopAndChangeState(new CharacterKickAttackState((OldCharacter)Creature, target.DirectionTo(Creature)));
            }
        }

        public IStateSnapshot Snapshot(OldCharacter character)
        {
            throw new NotImplementedException();
        }
    }
}