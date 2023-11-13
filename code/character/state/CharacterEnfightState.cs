using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures;
using y1000.code.player;
using y1000.code.player.state;
using y1000.code.util;

namespace y1000.code.character.state
{
    public class CharacterEnfightState : PlayerEnfightState, ICharacterState
    {
        private ICreature? target;

        public CharacterEnfightState(Character character, Direction direction, ICreature target) : base(character, direction)
        {
            this.target = target;
        }

        public void OnMouseMotion(Direction direction)
        {
            throw new NotImplementedException();
        }

        public void OnMouseRightClick(Direction clickDirection)
        {
            throw new NotImplementedException();
        }

        public void Process(double delta)
        {
            if (target == null)
            {
                return;
            }
            if (target.Coordinate.IsNextTo(Creature.Coordinate))
            {
                GD.Print("Attack now");
            }
        }

    }
}