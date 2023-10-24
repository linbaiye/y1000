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
        private static readonly Dictionary<int, Dictionary<Direction, int>> SWORD_SPRITE_OFFSET = new Dictionary<int, Dictionary<Direction, int>>()
        {
            {0, new Dictionary<Direction, int>()
                         {
            { Direction.UP, 0},
            { Direction.UP_RIGHT, 9},
            { Direction.RIGHT, 18},
            { Direction.DOWN_RIGHT, 27},
            { Direction.DOWN, 36},
            { Direction.DOWN_LEFT, 45},
            { Direction.LEFT, 54},
            { Direction.UP_LEFT, 63},
            }},

            {1, new Dictionary<Direction, int>()             {
            { Direction.UP, 144},
            { Direction.UP_RIGHT, 154},
            { Direction.RIGHT, 164},
            { Direction.DOWN_RIGHT, 174},
            { Direction.DOWN, 184},
            { Direction.DOWN_LEFT, 194},
            { Direction.LEFT, 204},
            { Direction.UP_LEFT, 214},
            }}
        };

        private static readonly Random RANDOM = new Random();
        private readonly Weapon weapon;
        private readonly int actionGroup;


        public AttackingState(Character character, Direction direction) : base(character, direction)
        {
            weapon = Weapon.SWORD;
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
            player.AddAnimationLibrary(GetLibraryName(Weapon.NONE, 0), CreateAnimations(7, 0.1f));
            player.AddAnimationLibrary(GetLibraryName(Weapon.NONE, 1), CreateAnimations(5, 0.1f));

            player.AddAnimationLibrary(GetLibraryName(Weapon.SWORD, 0), CreateAnimations(9, 0.1f));
            player.AddAnimationLibrary(GetLibraryName(Weapon.SWORD, 1), CreateAnimations(10, 0.1f));
        }

        private string GetLibraryName(Weapon w, int n)
        {
            return State.ToString() + "_" + w.ToString() + n;
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
                SpriteContainer container;
                Dictionary<int, Dictionary<Direction, int>> spriteGroup;

                switch (weapon)
                {
                    case Weapon.SWORD:
                        container = SpriteContainer.LoadMaleCharacterSprites("N00");
                        spriteGroup = SWORD_SPRITE_OFFSET;
                        break;
                    default:
                        container = SpriteContainer.LoadMaleCharacterSprites("N01");
                        spriteGroup = UNARMED_SPRITE_OFFSET;
                        break;
                }
                GD.Print(Character.PictureNumber);
                return container.Get(spriteGroup.GetValueOrDefault(actionGroup).GetValueOrDefault(Direction) + Character.PictureNumber);
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