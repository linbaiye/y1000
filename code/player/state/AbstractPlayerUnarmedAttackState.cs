using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code.creatures.state;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.Source.Sprite;

namespace y1000.code.player.state
{
    public abstract class AbstractPlayerUnarmedAttackState : AbstractPlayerAttackState
    {
        protected AbstractPlayerUnarmedAttackState(Player creature, Direction direction, AbstractCreatureStateFactory stateFactory,
            Dictionary<Direction, int> so) : base(creature, direction, stateFactory, so)
        {
        }

        protected override SpriteReader SpriteReader => ((IPlayer)Creature).IsMale() ? SpriteReader.LoadMalePlayerSprites("N01") : SpriteReader.EmptyReader;

        protected override string GetChestSpritePath(ChestArmor chestArmor)
        {
            return chestArmor.SpriteName + "1";
        }

        protected override string GetHatSpritePath(Hat hat)
        {
            return hat.SpriteBasePath + "1";
        }

        protected override string GetTrousersPath(Trousers trousers)
        {
            return trousers.SpriteBasePath + "1";
        }
    }
}