using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;

namespace y1000.code.player.state
{
    public class PlayerEnfightState : AbstractCreatureState, IPlayerState
    {
        private static readonly Dictionary<Direction, int> SPRITE_OFFSET = new Dictionary<Direction, int>()
        {

            { Direction.UP, 120},
			{ Direction.UP_RIGHT, 123},
			{ Direction.RIGHT, 126},
			{ Direction.DOWN_RIGHT, 129},
			{ Direction.DOWN, 132},
			{ Direction.DOWN_LEFT, 135},
			{ Direction.LEFT, 138},
			{ Direction.UP_LEFT, 141},

        };
        protected const int SPRITE_NUMBER = 3;

        protected const float STEP = 0.5f;
        
        public PlayerEnfightState(Player _player, Direction direction) : this(_player, direction, 1.0f) { }

        public PlayerEnfightState(Player player, Direction direction, float speed) : base(player, direction, PlayerStateFactory.INSTANCE)
        {
            player.AnimationPlayer.AddIfAbsent(State.ToString(), () => AnimationUtil.CreateAnimations(SPRITE_NUMBER, STEP, Godot.Animation.LoopModeEnum.Linear));
            player.AnimationPlayer.SpeedScale = speed;
        }

        protected override SpriteContainer SpriteContainer => ((Player)Creature).IsMale() ? SpriteContainer.LoadMalePlayerSprites("N02") : SpriteContainer.EmptyContainer;

        public override void OnAnimationFinised()
        {
            PlayAnimation();
        }

        public override void Turn(Direction newDirection)
        {
            SetDirection(newDirection);
        }

        public OffsetTexture ChestTexture(int animationSpriteNumber, IChestArmor armor)
        {
            string path = "armor/" + (armor.IsMale ? "male/": "female/") + "chest/" + armor.SpriteName + "0";
            return SpriteContainer.LoadSprites(path).Get(SPRITE_OFFSET.GetValueOrDefault(Direction, -1) + animationSpriteNumber);
        }

        public override State State => State.ENFIGHT;

        protected override int SpriteOffset => SPRITE_OFFSET.GetValueOrDefault(Direction, -1);

    }
}