using System.Drawing;
using System.Text.Json;
using Godot;
using y1000.code.networking;
using y1000.code.networking.message;
using y1000.code.player.state;
using y1000.code.util;
using y1000.Source.Creature;

namespace y1000.code.character.state.snapshot
{
    public class PositionSnapshot : IStateSnapshot
    {
        public Point Coordinate {get; set;}

        public Direction Direction {get; set;}

        public CreatureState State {get; set;}

        public bool Match(IUpdateStateMessage message)
        {
            return false;
        }

        public static PositionSnapshot ForState(IPlayerState playerState, OldCharacter character)
        {
            return new PositionSnapshot() { Coordinate  = character.Coordinate, Direction = character.Direction, State = playerState.State};
        }

        public override string? ToString() { return JsonSerializer.Serialize(this); }


    }
}