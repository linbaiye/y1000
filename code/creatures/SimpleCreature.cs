using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.creatures.state;
using y1000.code.monsters;

namespace y1000.code.creatures
{
    public partial class SimpleCreature : AbstractCreature
    {
        private SpriteContainer? spriteContainer;

        private Point initCoordinate = Point.Empty;

        private long id;

        private Direction initDirection;

        private void Initiliaze(Point i, SpriteContainer spriteContainer, long id, Direction direction)
        {
            initCoordinate = i;
            this.id = id;
            this.spriteContainer = spriteContainer;
            initDirection = direction;
        }

        public override long Id => id;

        public override void _Ready()
        {
            Setup();
            ChangeState(new SimpleCreatureIdleState(this, initDirection));
            ZIndex = 2;
            YSortEnabled = true;
            ZAsRelative = true;
            Coordinate = initCoordinate;
            CurrentState.PlayAnimation();
        }


        public static SimpleCreature Load(Point coordinate, long id, Direction direction)
        {
            PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/Monster.tscn");
            SimpleCreature buffalo = scene.Instantiate<SimpleCreature>();
            SpriteContainer spriteContainer = SpriteContainer.LoadMonsterSprites(MonsterNames.BUFFALO);
            buffalo.Initiliaze(coordinate, spriteContainer, id, direction);
            return buffalo;
        }

        public static SimpleCreature Load(Point coordinate, long id)
        {
            return Load(coordinate, id, Direction.DOWN);
        }

        public SpriteContainer SpriteContainer
        {
            get
            {
                if (spriteContainer != null)
                {
                    return spriteContainer;
                }
                throw new NotSupportedException();
            }
        }
    }
}