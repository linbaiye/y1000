using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Source.Networking.Protobuf;
using y1000.code.character;
using y1000.code.character.state;

namespace y1000.code.networking.message
{
    public class UpdateMovmentStateMessage : IUpdateCharacterStateMessage
    {
        public long Sequence { get; set; }

        public CreatureState ToState { get; set; }

        public long Id { get; set; }

        public Direction Direction {get; set;}

        public Point Coordinate {get; set;}

        public static UpdateMovmentStateMessage FromPacket(PositionPacket packet)
        {
            return new UpdateMovmentStateMessage()
            {
                Direction = (Direction)packet.Direction,
                Coordinate = new (packet.X, packet.Y),
                Sequence = 0,
                Id = packet.Id,
            };
        }

        public IOldCharacterState Restore(OldCharacter character)
        {
            character.Coordinate = Coordinate;
            return ToState switch
            {
                CreatureState.IDLE => new CharacterIdleState(character, Direction),
                CreatureState.WALK => new CharacterIdleState(character, Direction),
                _ => new CharacterIdleState(character, Direction),
            };
        }

        public override string? ToString() { return JsonSerializer.Serialize(this); }

    }
}