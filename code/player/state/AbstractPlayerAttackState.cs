using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;

namespace y1000.code.player.state
{
    public abstract class AbstractPlayerAttackState : AbstractCreatureState, IPlayerState
    {
        public override State State => State.ATTACKING;

        private readonly Dictionary<Direction, int> spriteOffset;

        protected AbstractPlayerAttackState(Player creature,
         Direction direction,
         AbstractCreatureStateFactory _stateFactory,
         Dictionary<Direction, int> so) : base(creature, direction, _stateFactory)
        {
            spriteOffset = so;
        }

        public override void OnAnimationFinised()
        {
            StopAndChangeState(new PlayerEnfightState((Player)Creature, Direction));
        }

        protected override int SpriteOffset => spriteOffset.GetValueOrDefault(Direction, -1);

        protected abstract string GetChestSpritePath(ChestArmor armor);

        protected abstract string GetHatSpritePath(Hat hat);

        protected abstract string GetTrousersPath(Trousers trousers);

        public OffsetTexture ChestTexture(int animationSpriteNumber, ChestArmor armor)
        {
            string path = "armor/" + (armor.IsMale ? "male/": "female/") + "chest/" + GetChestSpritePath(armor);
            return SpriteContainer.LoadSprites(path).Get(SpriteOffset + animationSpriteNumber);
        }

        public OffsetTexture HatTexture(int animationSpriteNumber, Hat hat)
        {
            return SpriteContainer.LoadSprites(GetHatSpritePath(hat)).Get(SpriteOffset + animationSpriteNumber);
        }

        public OffsetTexture TrousersTexture(int animationSpriteNumber, Trousers trousers)
        {
            return SpriteContainer.LoadSprites(GetTrousersPath(trousers)).Get(SpriteOffset + animationSpriteNumber);
        }
    }
}