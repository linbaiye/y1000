using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;

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

        protected abstract string FullArmorSpriteName(IChestArmor armor);

        public OffsetTexture ChestTexture(int animationSpriteNumber, IChestArmor armor)
        {
            string path = "armor/" + (armor.IsMale ? "male/": "female/") + "chest/" + FullArmorSpriteName(armor);
            return SpriteContainer.LoadSprites(path).Get(SpriteOffset + animationSpriteNumber);
        }
    }
}