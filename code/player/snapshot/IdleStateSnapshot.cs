using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.util;

namespace y1000.code.player.snapshot
{
    public class IdleStateSnapshot : ISnapshot
    {

        private readonly long stateStartAt;

        private readonly long snapshotStartAt;

        private readonly long duration;

        private const long STATE_DURATION = 3000;


        public IdleStateSnapshot(long stateStart, long snapshotStart, long d)
        {
            stateStartAt = stateStart;
            snapshotStartAt = snapshotStart;
            duration = d;
        }

        private static readonly Dictionary<Direction, int> STATE_SPRITE_OFFSET = new()
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

        private static readonly SpriteContainer MALE_SPRITES = SpriteContainer.LoadMalePlayerSprites("N02");

        private static readonly Dictionary<int, int> MILLIS_SPIRTE_MAP = new Dictionary<int, int>() {
            {500, 0},
            {1000, 1},
            {1500, 2},
            {2000, 2},
            {2500, 1},
            {3000, 0},
        };


        private static readonly List<TimeMillisSprite> MILLIS_SPRITE_RANGE = new List<TimeMillisSprite>() 
        {
            new TimeMillisSprite(500, 0),
            new TimeMillisSprite(1000, 1),
            new TimeMillisSprite(1500, 2),
            new TimeMillisSprite(2000, 2),
            new TimeMillisSprite(2500, 1),
            new TimeMillisSprite(3000, 0),
        };


        private class TimeMillisSprite
        {
            public int Millis {get; set;}

            public int SpriteNumber {get; set;}

            public TimeMillisSprite(int m, int s) 
            {
                Millis = m;
                SpriteNumber = s;
            }
        }

        public void CreateAnimation(AnimationPlayer animationPlayer)
        {
            /*if (animationPlayer.HasAnimationLibrary("test")) {
                return;
            }
            Animation animation = new Animation();
            int trackIdx = animation.AddTrack(Animation.TrackType.Value);
            animation.TrackSetPath(trackIdx, ".:metadata/spriteNumber");
            float time = 0.0f;
            for (long start = snapshotStartAt - stateStartAt; start < duration; start += 50, time += 0.05f)
            {
                int spirteNumber = MILLIS_SPIRTE_MAP.GetValueOrDefault((int)(start % STATE_DURATION), 0);
                animation.TrackInsertKey(trackIdx, time, spirteNumber);
            }
            animation.Length = (float)duration / 1000;

            AnimationLibrary library = new AnimationLibrary();
            library.AddAnimation("test", animation);
            animationPlayer.AddAnimationLibrary("test", library);*/
        }


        public bool DurationEnough(long elapsed)
        {
            return elapsed - snapshotStartAt >= duration;
        }


        private int GetSpriteNumber(long elapsed)
        {
            var currentTime = elapsed - stateStartAt;
            var spriteTime = currentTime % STATE_DURATION;
            foreach ( var tmp in  MILLIS_SPRITE_RANGE )
            {
                if (spriteTime <= tmp.Millis) 
                {
                    return tmp.SpriteNumber;
                }
            }
            return 0;
        }

        public OffsetTexture BodyTexture(OtherPlayer player, long elapsed)
        {
            //LOG.Debug("Sprite time:  " + spriteTime);
            int spriteNumber = GetSpriteNumber(elapsed);
            int offset = STATE_SPRITE_OFFSET.GetValueOrDefault(player.Direction);
            //LOG.Debug("Sprite number:  " + spriteNumber + offset);
            return MALE_SPRITES.Get(offset + spriteNumber);
        }
    }
}