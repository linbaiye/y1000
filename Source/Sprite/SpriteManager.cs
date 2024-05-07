using System;
using System.Collections.Generic;
using System.Linq;
using y1000.code;
using y1000.code.player;
using y1000.Source.Creature;
using y1000.Source.Entity.Animation;

namespace y1000.Source.Sprite
{
    public class SpriteManager
    {
        private readonly struct PeriodSpritePair
        {
            public PeriodSpritePair(long end, int frameOffset)
            {
                Time = end;
                SpriteOffset = frameOffset;
            }

            public long Time { get; }

            public int SpriteOffset { get; }
        }

        private readonly List<PeriodSpritePair> _mappers;

        private readonly int _totalMillis;

        private readonly Dictionary<Direction, int> _directionOffsetMap;

        private readonly SpriteReader _spriteReader;

        private SpriteManager(List<PeriodSpritePair> mappers, int totalMillis, Dictionary<Direction, int> directionOffsetMap, SpriteReader spriteReader)
        {
            _mappers = mappers;
            _totalMillis = totalMillis;
            _directionOffsetMap = directionOffsetMap;
            _spriteReader = spriteReader;
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
            return _spriteReader.Get(_directionOffsetMap.GetValueOrDefault(direction) + frameOffset);
        }

        public int AnimationLength => _totalMillis;

        private static List<PeriodSpritePair> CreateFrameMappers(long spriteLastMillis, int total, bool pingpong)
        {
            List<PeriodSpritePair> result = new List<PeriodSpritePair>();
            for (int i = 0; i < total; i++)
            {
                result.Add(new PeriodSpritePair(i * spriteLastMillis, i));
            }
            if (pingpong)
            {
                for (int i = 0; i < total; i++)
                {
                    result.Add(new PeriodSpritePair((i + total) * spriteLastMillis, total - i - 1));
                }
            }
            return result;
        }

        private static SpriteManager Create(
            int millisPerSprite,
            Dictionary<Direction, int> directionOffSetMap,
            SpriteReader spriteReader, bool pingpong)
        {
            var offsets = new List<int>(directionOffSetMap.Values.OrderBy(p => p));
            var spriteNumber = offsets[1] - offsets[0];
            return new SpriteManager(CreateFrameMappers(millisPerSprite, spriteNumber, pingpong), spriteNumber * millisPerSprite * (pingpong ? 2 : 1), directionOffSetMap, spriteReader);
        }
        
        
        private static SpriteManager Create(
            int millisPerSprite,
            Dictionary<Direction, int> directionOffSetMap,
            SpriteReader spriteReader, bool pingpong, int spriteNumber)
        {
            return new SpriteManager(CreateFrameMappers(millisPerSprite, spriteNumber, pingpong), spriteNumber * millisPerSprite * (pingpong ? 2 : 1), directionOffSetMap, spriteReader);
        }


        private static SpriteManager Normal(
            int spriteLengthMillis,
            Dictionary<Direction, int> directionOffSetMap,
            SpriteReader spriteReader)
        {
            return Create(spriteLengthMillis, directionOffSetMap, spriteReader, false);
        }


        private static SpriteManager WithPinpong(
            int spriteLengthMillis,
            Dictionary<Direction, int> directionOffSetMap,
            SpriteReader spriteReader)
        {
            return Create(spriteLengthMillis, directionOffSetMap, spriteReader, true);
        }

        
        private static readonly Dictionary<Direction, int> MALE_IDLE_DIRECTION_SPRITE_OFFSET = new()
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
        
        private static readonly Dictionary<Direction, int> MALE_WALK_DIRECTION_SPRITE_OFFSET = new()
        {
            { Direction.UP, 0},
            { Direction.UP_RIGHT, 6},
            { Direction.RIGHT, 12},
            { Direction.DOWN_RIGHT, 18},
            { Direction.DOWN, 24},
            { Direction.DOWN_LEFT, 30},
            { Direction.LEFT, 36},
            { Direction.UP_LEFT, 42},
        };
        
        
        private static readonly Dictionary<Direction, int> DEFAULT_SPRITE_OFFSET = new()
        {
            { Direction.UP, 18},
            { Direction.UP_RIGHT, 41},
            { Direction.RIGHT, 64},
            { Direction.DOWN_RIGHT, 87},
            { Direction.DOWN, 110},
            { Direction.DOWN_LEFT, 133},
            { Direction.LEFT, 156},
            { Direction.UP_LEFT, 179},
        };
        
        private static readonly Dictionary<Direction, int> MONSTER_WALK_SPRITE_OFFSET = new()
        {
            { Direction.UP, 0},
			{ Direction.UP_RIGHT, 23},
			{ Direction.RIGHT, 46},
			{ Direction.DOWN_RIGHT, 69},
			{ Direction.DOWN, 92},
			{ Direction.DOWN_LEFT, 115},
			{ Direction.LEFT, 138},
			{ Direction.UP_LEFT, 161},
        };
        
        private static readonly Dictionary<Direction, int> BELOW50_FIST_OFFSET = new()
        {
            { Direction.UP, 55},
            { Direction.UP_RIGHT, 60},
            { Direction.RIGHT, 65},
            { Direction.DOWN_RIGHT, 70},
            { Direction.DOWN, 75},
            { Direction.DOWN_LEFT, 80},
            { Direction.LEFT, 85},
            { Direction.UP_LEFT, 90},
        };
        
        private static readonly Dictionary<Direction, int> ABOVE50_FIST_SPRITE = new()
        {
            { Direction.UP, 0},
            { Direction.UP_RIGHT, 6},
            { Direction.RIGHT, 13},
            { Direction.DOWN_RIGHT, 20},
            { Direction.DOWN, 27},
            { Direction.DOWN_LEFT, 34},
            { Direction.LEFT, 41},
            { Direction.UP_LEFT, 48}
        };
        
        
        private static readonly Dictionary<Direction, int> PLAYER_COOLDOWN = new()
        {
            { Direction.UP, 120},
			{ Direction.UP_RIGHT, 123},
			{ Direction.RIGHT, 126},
			{ Direction.DOWN_RIGHT, 129},
			{ Direction.DOWN, 132},
			{ Direction.DOWN_LEFT, 135},
			{ Direction.LEFT, 138},
			{ Direction.UP_LEFT, 141},
        };
        
        private static readonly Dictionary<Direction, int> MONSTER_HURT_SPRITE_OFFSET = new()
        {
            { Direction.UP, 12},
            { Direction.UP_RIGHT, 35},
            { Direction.RIGHT, 58},
            { Direction.DOWN_RIGHT, 81},
            { Direction.DOWN, 104},
            { Direction.DOWN_LEFT, 127},
            { Direction.LEFT, 150},
            { Direction.UP_LEFT, 173},
        };


        

        private static SpriteManager LoadMalePlayer(CreatureState state)
        {
            return state switch
            {
                CreatureState.IDLE => WithPinpong(500, MALE_IDLE_DIRECTION_SPRITE_OFFSET,
                    SpriteReader.LoadOffsetMalePlayerSprites("N02")),
                CreatureState.WALK => Create(150, MALE_WALK_DIRECTION_SPRITE_OFFSET, SpriteReader.LoadOffsetMalePlayerSprites("N02"), false, 6),
                CreatureState.RUN => Create(75, MALE_WALK_DIRECTION_SPRITE_OFFSET, SpriteReader.LoadOffsetMalePlayerSprites("N02"), false, 6),
                CreatureState.FLY => Create(75, MALE_IDLE_DIRECTION_SPRITE_OFFSET, SpriteReader.LoadOffsetMalePlayerSprites("N02"), true, 3),
                _ => throw new NotSupportedException()
            };
        }

        public static SpriteManager LoadPlayerCooldown(bool male, int millis)
        {
            return
                Create(millis, PLAYER_COOLDOWN, SpriteReader.LoadOffsetMalePlayerSprites("N02"), false, 3);
        }
        

        public static SpriteManager LoadQuanfaAttack(bool male, bool below50, int millisPerSprite)
        {
            var sprites = SpriteReader.LoadOffsetMalePlayerSprites("N01");
            return below50
                ? Create(millisPerSprite, BELOW50_FIST_OFFSET, sprites, false, 5)
                : Create(millisPerSprite, ABOVE50_FIST_SPRITE, sprites, false, 6);
        }

        private static SpriteManager LoadFemalePlayer(CreatureState state)
        {
            throw new NotSupportedException();
        }

        public static SpriteManager LoadForPlayer(bool male, CreatureState state)
        {
            return male ? LoadMalePlayer(state) : LoadFemalePlayer(state);
        }

        private static readonly Dictionary<string, string> CREATURE_NAME_TO_DIR = new()
        {
            { "牛", "buffalo" }
        };

        public static SpriteManager LoadForMonster(string name, CreatureState state)
        {
            var dir = CREATURE_NAME_TO_DIR.GetValueOrDefault(name, "buffalo");
            return state switch
            {
                CreatureState.IDLE => Create(400, DEFAULT_SPRITE_OFFSET, SpriteReader.LoadOffsetMonsterSprites(dir), false, 5),
                CreatureState.WALK => Create(150, MONSTER_WALK_SPRITE_OFFSET, SpriteReader.LoadOffsetMonsterSprites(dir), false, 7),
                CreatureState.HURT => Create(150, MONSTER_HURT_SPRITE_OFFSET, SpriteReader.LoadOffsetMonsterSprites(dir), false, 3),
                _ => throw new NotSupportedException()
            };
        }
    }
}