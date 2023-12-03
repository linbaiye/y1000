using System.Drawing;
using y1000.code.networking;
using y1000.code.networking.message;
using y1000.code.player.state;

namespace y1000.code.character.state.snapshot
{
    public class PositionSnapshot : IStateSnapshot
    {
        public Point Coordinate {get; set;}

        public Direction Direction {get; set;}

        public State State {get; set;}

        public bool Match(IUpdateStateMessage message)
        {
            if (message is UpdateMovmentStateMessage movmentStateMessage)
            {
                return movmentStateMessage.Direction == Direction &&
                 movmentStateMessage.Coordinate.Equals(Coordinate) &&
                 State == movmentStateMessage.ToState;
            }
            return false;
        }

        public static PositionSnapshot ForState(IPlayerState playerState, Character character)
        {
            return new PositionSnapshot() { Coordinate  = character.Coordinate, Direction = character.Direction, State = playerState.State};
        }


    }
}