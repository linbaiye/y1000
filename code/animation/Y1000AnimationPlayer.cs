using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.animation
{
    public partial class Y1000AnimationPlayer: AnimationPlayer
    {

        public override void _Ready()
        {
        }

        public void DeregisterCallback(string animationName, int trackId)
        {
            if (!HasAnimation(animationName))
            {
                throw new NotSupportedException();
            }
            Animation animation = GetAnimation(animationName);
            animation.RemoveTrack(trackId);
        }

        public int RegisterCallback(string animationName, double atTime, Action callback)
        {
            if (!HasAnimation(animationName))
            {
                throw new NotSupportedException();
            }
            Animation animation = GetAnimation(animationName);
            int index = animation.AddTrack(Animation.TrackType.Method);
            animation.TrackSetPath(index, "./AnimationPlayer");
            animation.TrackInsertKey(index, atTime, new Godot.Collections.Dictionary<string, Variant>() {
            {"method", "OnCallback"},
            {"args", new Godot.Collections.Array(){Callable.From(callback)}}
        });
            return index;
        }


        public void OnCallback(Callable callable)
        {
            callable.Call();
        }
    }
}