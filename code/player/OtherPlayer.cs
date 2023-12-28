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

        private readonly Queue<ISnapshot> stateSnapshots = new();

        private ISnapshot? currentSnapshot;

        public Direction Direction {get; set;}

        public override void _Ready()
        {
            createdAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            LOG.Debug("Other player ready");
        }

        public void EnqueueSnapshot(ISnapshot stateSnapshot)
        {
            stateSnapshots.Enqueue(stateSnapshot);
        }

        public override void _Process(double delta)
        {
            elapsed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (currentSnapshot != null)
            {
                if (!currentSnapshot.DurationEnough(elapsed))
                {
                    return;
                }
                currentSnapshot = null;
            }
            if (stateSnapshots.Any())
            {
                currentSnapshot = stateSnapshots.Dequeue();
            }
        }


        public OffsetTexture? BodyTexutre => currentSnapshot?.BodyTexture(this, elapsed);

        public static OtherPlayer Test()
        {
            PackedScene scene = ResourceLoader.Load<PackedScene>("res://scene/OtherPlayer.tscn");
            var player = scene.Instantiate<OtherPlayer>();
            for (int i = 0; i < 6000; i += 50)
            {
                player.EnqueueSnapshot(new IdleStateSnapshot(0, i, 50));
            }
            player.Direction = Direction.RIGHT;
            return player;
        }

        public static OtherPlayer CreatePlayer(ShowPlayerMessage message)
        {
            return new OtherPlayer()
            {
                 Direction = message.MovmentStateMessage.Direction
            };
        }
    }
}