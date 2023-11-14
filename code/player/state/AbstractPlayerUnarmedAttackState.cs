using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;

namespace y1000.code.player.state
{
    public abstract class AbstractPlayerUnarmedAttackState : AbstractPlayerAttackState
    {
        protected AbstractPlayerUnarmedAttackState(Player creature, Direction direction, AbstractCreatureStateFactory stateFactory,
            Dictionary<Direction, int> so) : base(creature, direction, stateFactory, so)
        {
        }

        protected override SpriteContainer SpriteContainer => ((IPlayer)Creature).IsMale() ? SpriteContainer.LoadMalePlayerSprites("N01") : SpriteContainer.EmptyContainer;

        protected override string FullArmorSpriteName(IChestArmor chestArmor)
        {
            return chestArmor.SpriteName + "1";
        }
    }
}