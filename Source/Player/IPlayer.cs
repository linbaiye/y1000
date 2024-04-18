using Godot;
using y1000.Source.Creature;

namespace y1000.Source.Player;

public interface IPlayer : ICreature
{
    bool IsMale { get; }
}