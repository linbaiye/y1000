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

    public PlayerImpl CreatePlayer(PlayerInterpolation playerInterpolation, IMap map)
    {
        PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/player.tscn");
        var player = scene.Instantiate<PlayerImpl>();
        var interpolation = playerInterpolation.Interpolation;
        var state = IPlayerState.CreateFrom(playerInterpolation);
        player.Init(playerInterpolation.Male, state, 
            interpolation.Direction, interpolation.Coordinate, playerInterpolation.Id, map);
        if (playerInterpolation.WeaponName != null)
        {
            var weapon = _itemFactory.CreatePlayerWeapon(playerInterpolation.WeaponName);
            player.ChangeWeapon(weapon);
        }
        if (state is PlayerMoveState moveState)
        {
            moveState.Init(player);
        }
        return player;
    }

}