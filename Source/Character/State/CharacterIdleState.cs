using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using y1000.code;
using y1000.code.character.state.input;
using y1000.code.character.state.Prediction;
using y1000.code.player;
using y1000.code.util;

namespace y1000.Source.Character.State
{
    public class CharacterIdleState : ICharacterState
    {

        private int _elpasedMillis;

        private const int ANIMATION_CYCLE_MILLIS = 1500;

        private readonly IEnumerable<TimeFrameMapper> _frameMappers;

        private static readonly List<TimeFrameMapper> FRAME_MAPPERS = CreateFrameMappers(300, 3, true);

        public static List<TimeFrameMapper> CreateFrameMappers(double interval, int total, bool pingpong)
        {
            List<TimeFrameMapper> result = new List<TimeFrameMapper>();
            for (int i = 0; i < total; i++)
            {
                result.Add(new TimeFrameMapper(i * interval, i));
            }
            if (pingpong)
            {
                for (int i = 0; i < total; i++)
                {
                    result.Add(new TimeFrameMapper((i + total) * interval, total - i - 1));
                }
            }
            return result;
        }

        private static readonly Dictionary<Direction, int> BODY_SPRITE_OFFSET = new()
        {
            { Direction.UP, 48},
			{ Direction.UP_RIGHT, 51},
			{ Direction.RIGHT, 54},
			{ Direction.DOWN_RIGHT, 57},
			{ Direction.DOWN, 60},
			{ Direction.DOWN_LEFT, 63},
			{ Direction.LEFT, 66},
			{ Direction.UP_LEFT, 69},
        };

        public CharacterIdleState()
        {
            _frameMappers = FRAME_MAPPERS;
        }

        public void OnMouseRightClicked(Character character, MouseRightClick rightClick)
        {
            LOG.Debug("Right clicked.");
        }


        public IPrediction Predict(Character character, MouseRightClick rightClick)
        {
            return new MovedPrediction(rightClick, character.Coordinate);
        }


        public void Process(Character character, double delta)
        {
            _elpasedMillis += (int)(delta * 1000);
        }


        public OffsetTexture BodyOffsetTexture(Character character)
        {
            SpriteContainer SpriteContainer = character.IsMale ? SpriteContainer.LoadMalePlayerSprites("N02") : SpriteContainer.EmptyContainer;
            var millis = _elpasedMillis % ANIMATION_CYCLE_MILLIS;
            int frameOffset = 0;
            foreach (var m in _frameMappers)
            {
                if (millis < m.Time)
                {
                    frameOffset = m.FrameOffset;
                    break;
                }
            }
            return SpriteContainer.Get(BODY_SPRITE_OFFSET.GetValueOrDefault(character.Direction) + frameOffset);
        }
    }
}