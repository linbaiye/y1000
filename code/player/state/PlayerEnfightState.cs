using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.character;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;
using y1000.code.player.skill;
using y1000.Source.Sprite;

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

        protected override SpriteReader SpriteReader => ((Player)Creature).IsMale() ? SpriteReader.LoadMalePlayerSprites("N02") : SpriteReader.EmptyReader;

        public override void OnAnimationFinised()
        {
            PlayAnimation();
        }

        public override void Turn(Direction newDirection)
        {
            SetDirection(newDirection);
        }

        public OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor)
        {
            return SpriteReader.LoadSprites(armor.SpriteBasePath + "0").Get(SpriteOffset + animationSpriteNumber);
        }

        public OffsetTexture HatTexture(int animationSpriteNumber, Hat hat)
        {
            return SpriteReader.LoadSprites(hat.SpriteBasePath + "0").Get(SpriteOffset + animationSpriteNumber);
        }

        public OffsetTexture TrousersTexture(int animationSpriteNumber, Trousers trousers)
        {
            return SpriteReader.LoadSprites(trousers.SpriteBasePath + "0").Get(SpriteOffset + animationSpriteNumber);
        }

        private void OnHurtDone()
        {
            Creature.ChangeState(this);
            PlayAnimation();
        }


        public override void Hurt()
        {
            StopAndChangeState(new PlayerStandHurtState(Creature, Direction, OnHurtDone));
        }

        public bool PressBufa(IBufa bufa)
        {
            StopAndChangeState(new PlayerIdleState((OldCharacter)Creature, Direction));
            return true;
        }

        public OffsetTexture WeaponTexture(int animationSpriteNumber, IWeapon weapon)
        {
            throw new NotImplementedException();
        }

        public override CreatureState State => CreatureState.ENFIGHT;

        protected override int SpriteOffset => SPRITE_OFFSET.GetValueOrDefault(Direction, -1);

    }
}