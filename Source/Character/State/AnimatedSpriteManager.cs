using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using y1000.code;
using y1000.code.player;

namespace y1000.Source.Character.State
{
    public class AnimatedSpriteManager
    {
        private readonly struct TimeSpriteMapper
        {
            public TimeSpriteMapper(long end, int frameOffset)
            {
                Time = end;
                SpriteOffset = frameOffset;
            }

            public long Time { get; }

            public int SpriteOffset { get; }
        }

        private readonly List<TimeSpriteMapper> _mappers;

        private readonly int _totalMillis;

        private readonly Dictionary<Direction, int> _directionOffsetMap;

        private readonly SpriteContainer _spriteContainer;

        private AnimatedSpriteManager(List<TimeSpriteMapper> mappers, int totalMillis, Dictionary<Direction, int> directionOffsetMap, SpriteContainer spriteContainer)
        {
            _mappers = mappers;
            _totalMillis = totalMillis;
            _directionOffsetMap = directionOffsetMap;
            _spriteContainer = spriteContainer;
        }


        public OffsetTexture Texture(Direction direction, long currentMillis)
        {
            var millis = currentMillis % _totalMillis;
            int frameOffset = 0;
            foreach (var m in _mappers)
            {
                if (millis < m.Time)
                {
                    frameOffset = m.SpriteOffset;
                    break;
                }
            }
            return _spriteContainer.Get(_directionOffsetMap.GetValueOrDefault(direction) + frameOffset);
        }

        public int AnimationLength => _totalMillis;

        private static List<TimeSpriteMapper> CreateFrameMappers(long spriteLastMillis, int total, bool pingpong)
        {
            List<TimeSpriteMapper> result = new List<TimeSpriteMapper>();
            for (int i = 0; i < total; i++)
            {
                result.Add(new TimeSpriteMapper(i * spriteLastMillis, i));
            }
            if (pingpong)
            {
                for (int i = 0; i < total; i++)
                {
                    result.Add(new TimeSpriteMapper((i + total) * spriteLastMillis, total - i - 1));
                }
            }
            return result;
        }

        private static AnimatedSpriteManager Create(
            int spriteLengthMillis,
            Dictionary<Direction, int> directionOffSetMap,
            SpriteContainer spriteContainer, bool pingpong)
        {
            var offsets = new List<int>(directionOffSetMap.Values.OrderBy(p => p));
            int spriteNumber = offsets[1] - offsets[0];
            return new AnimatedSpriteManager(CreateFrameMappers(spriteLengthMillis, spriteNumber, pingpong), spriteNumber * spriteLengthMillis, directionOffSetMap, spriteContainer);
        }


        public static AnimatedSpriteManager Normal(
            int spriteLengthMillis,
            Dictionary<Direction, int> directionOffSetMap,
            SpriteContainer spriteContainer)
        {
            return Create(spriteLengthMillis, directionOffSetMap, spriteContainer, false);
        }


        public static AnimatedSpriteManager WithPinpong(
            int spriteLengthMillis,
            Dictionary<Direction, int> directionOffSetMap,
            SpriteContainer spriteContainer)
        {
            return Create(spriteLengthMillis, directionOffSetMap, spriteContainer, true);
        }
    }
}