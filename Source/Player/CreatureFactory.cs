using Godot;
using y1000.Source.Character.State;
using y1000.Source.Creature;
using y1000.Source.Item;
using y1000.Source.Map;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;

namespace y1000.Source.Player;

public class CreatureFactory
{
    private readonly ItemFactory _itemFactory;

    public CreatureFactory()
    {
        _itemFactory = ItemFactory.Instance;
    }

}