using System.Xml.Linq;
using y1000.code.player;
using y1000.Source.Creature.State;

namespace y1000.Source.Player;

public interface IPlayerState : ICreatureState<Player>
{
    static readonly IPlayerState Empty = EmptyPlayerState.Instance;
}