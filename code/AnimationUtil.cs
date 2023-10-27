using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code
{
    public static class AnimationUtil
    {

        public static AnimationLibrary CreateAnimations(int total, float step, Animation.LoopModeEnum loopModeEnum)
        {
            AnimationLibrary library = new AnimationLibrary();
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var animation = CreateAnimation(total, step);
                animation.LoopMode = loopModeEnum;
                library.AddAnimation(direction.ToString(), animation);
            }
            return library;
        }


        public static Animation CreateAnimation(int total, float step)
        {
            Animation animation = new Animation();
            int trackIdx = animation.AddTrack(Animation.TrackType.Value);
            animation.TrackSetPath(trackIdx, ".:metadata/spriteNumber");
            float time = 0.0f;
            for (int i = 0; i <= total; i++, time += step)
            {
                animation.TrackInsertKey(trackIdx, time, Math.Min(i, total - 1));
            }
            animation.Length = total * step;
            return animation;
        }

        public static void AddIfAbsent(this AnimationPlayer animationPlayer, string name, Func<AnimationLibrary> library)
        {
            if (!animationPlayer.HasAnimationLibrary(name))
            {
                animationPlayer.AddAnimationLibrary(name, library.Invoke());
            }
        }
    }
}