using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using NLog.Fluent;
using y1000.code.character.state;
using y1000.code.networking.message;
using y1000.code.player.snapshot;
using y1000.code.util;

namespace y1000.code.player
{
    public partial class OtherPlayer : Node2D
    {
        private long createdAt;

        private long elapsed;

        private readonly Queue<IInterpolation> stateSnapshots = new();

        private IInterpolation? currentSnapshot;

        public override void _Ready()
        {
            createdAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public void EnqueueInterpolation(IInterpolation stateSnapshot)
        {
            stateSnapshots.Enqueue(stateSnapshot);
        }

        public override void _Process(double delta)
        {
            elapsed = DateTimeOffset.Now.ToUnixTimeMilliseconds() - createdAt;
            if (currentSnapshot != null)
            {
                Position = currentSnapshot.Position;
                if (!currentSnapshot.DurationEnough(elapsed))
                {
                    return;
                }
                currentSnapshot = null;
            }
            if (stateSnapshots.Any())
            {
                currentSnapshot = stateSnapshots.Dequeue();
                Position = currentSnapshot.Position;
            }
        }


        public OffsetTexture? BodyTexutre => currentSnapshot?.BodyTexture(this, elapsed);

        public static OtherPlayer Test()
        {
            PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/OtherPlayer.tscn");
            var player = scene.Instantiate<OtherPlayer>();
            for (int i = 0; i < 60000; i += 50)
            {
                player.EnqueueInterpolation(new IdleInterpolation(0, i, 50));
            }
            return player;
        }

        public static OtherPlayer CreatePlayer(IInterpolation interpolation)
        {
            PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/OtherPlayer.tscn");
            var player = scene.Instantiate<OtherPlayer>();
            player.EnqueueInterpolation(interpolation);
            return player;
        }
    }
}