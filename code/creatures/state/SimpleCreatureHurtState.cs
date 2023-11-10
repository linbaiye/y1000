using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.creatures.state
{
    public class SimpleCreatureHurtState : AbstractCreatureHurtState
    {
        public SimpleCreatureHurtState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
        }
    }
}