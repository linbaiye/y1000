using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.creatures.state
{
    public sealed class SimpleCreatureHurtState : AbstractCreatureHurtState
    {
        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new()
        {
            { Direction.UP, 12},
            { Direction.UP_RIGHT, 35},
            { Direction.RIGHT, 58},
            { Direction.DOWN_RIGHT, 81},
            { Direction.DOWN, 104},
            { Direction.DOWN_LEFT, 127},
            { Direction.LEFT, 150},
            { Direction.UP_LEFT, 173},
        };

        public SimpleCreatureHurtState(AbstractCreature creature, Direction direction) : base(creature, direction)
        {
        }

        protected override SpriteContainer SpriteContainer => ((SimpleCreature)Creature).SpriteContainer;

        protected override int SpriteOffset => SPRITE_OFFSET.GetValueOrDefault(Direction, -1);
    }
}