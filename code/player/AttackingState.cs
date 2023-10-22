using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.player
{
    public class AttackingState : AbstractPlayerState
    {
        private static readonly Dictionary<int, Dictionary<Direction, int>> UNARMED_SPRITE_OFFSET = new Dictionary<int, Dictionary<Direction, int>>()
        {
            {0, new Dictionary<Direction, int>()
                         {
            { Direction.UP, 0},
            { Direction.UP_RIGHT, 7},
            { Direction.RIGHT, 14},
            { Direction.DOWN_RIGHT, 21},
            { Direction.DOWN, 28},
            { Direction.DOWN_LEFT, 35},
            { Direction.LEFT, 42},
            { Direction.UP_LEFT, 49},
            }},

            {1, new Dictionary<Direction, int>()             {
            { Direction.UP, 56},
            { Direction.UP_RIGHT, 61},
            { Direction.RIGHT, 66},
            { Direction.DOWN_RIGHT, 71},
            { Direction.DOWN, 76},
            { Direction.DOWN_LEFT, 81},
            { Direction.LEFT, 86},
            { Direction.UP_LEFT, 91},
            }}
        };

        private static readonly Random RANDOM = new Random();
        private readonly Weapon weapon;
        private readonly int actionGroup;


        public AttackingState(Character character, Direction direction) : base(character, direction)
        {
            weapon = Weapon.NONE;
            SetupAnimations();
            actionGroup = RANDOM.Next(2);
            Character.AnimationPlayer.Play(GetLibraryName(actionGroup) + "/" + Direction);
        }

        private enum Weapon
        {
            NONE,
            SWORD,
            BLADE,

            AEX,
            SPEAR,
        }

        private void SetupAnimations()
        {
            if (Character.AnimationPlayer.HasAnimationLibrary(GetLibraryName(0)))
            {
                return;
            }
            var player = Character.AnimationPlayer;
            player.AddAnimationLibrary(GetLibraryName(0), CreateAnimations(7, 0.1f));
            player.AddAnimationLibrary(GetLibraryName(1), CreateAnimations(5, 0.1f));
        }

        private string GetLibraryName(int n)
        {
            return State.ToString() + "_" + weapon.ToString() + n;
        }

        public override State State => State.ATTACKING;

        public override PositionedTexture BodyTexture
        {
            get
            {
                var container = SpriteContainer.LoadMaleCharacterSprites("N01");
                return container.Get(UNARMED_SPRITE_OFFSET.GetValueOrDefault(actionGroup).GetValueOrDefault(Direction) + Character.PictureNumber);
            }
        }

        public override void PhysicsProcess(double delta)
        {

        }

        public override void OnAnimationFinished(StringName animationName) 
        {
            Character.ChangeState(new EnfightState(Character, Direction));
        }

    }
}