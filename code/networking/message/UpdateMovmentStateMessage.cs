using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Code.Networking.Gen;
using y1000.code.character;
using y1000.code.character.state;
using y1000.code.util;

namespace y1000.code.networking.message
{
    public class UpdateMovmentStateMessage : IUpdateCharacterStateMessage
    {
        public long Sequence { get; set; }

        public State ToState { get; set; }

        public int Id { get; set; }

        public long Timestamp { get; set; }

        public Direction Direction {get; set;}

        public Point Coordinate {get; set;}

        public static UpdateMovmentStateMessage FromPacket(MovementPacket packet)
        {
            return new UpdateMovmentStateMessage()
            {
                ToState = (State)packet.State,
                Direction = (Direction)packet.Direction,
                Coordinate = new (packet.X, packet.Y),
                Sequence = packet.Sequence,
                Id = packet.Id,
                Timestamp = packet.Timestamp
            };
        }

        public ICharacterState Restore(Character character)
        {
            character.Coordinate = Coordinate;
            return ToState switch
            {
                State.IDLE => new CharacterIdleState(character, Direction),
                State.WALK => new CharacterIdleState(character, Direction),
                _ => new CharacterIdleState(character, Direction),
            };
        }

        public override string? ToString() { return JsonSerializer.Serialize(this); }

    }
}