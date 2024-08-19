using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.Source.Animation;
using y1000.Source.Creature;
using y1000.Source.Sprite;

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

        protected override AtzSprite AtzSprite => ((SimpleCreature)Creature).AtzSprite;

        protected override int SpriteOffset => SPRITE_OFFSET.GetValueOrDefault(Direction, -1);
    }
}