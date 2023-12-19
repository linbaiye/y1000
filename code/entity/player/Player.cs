using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using y1000.code.entity.equipment.chest;
using y1000.code.entity.equipment.hat;
using y1000.code.entity.equipment.trousers;
using y1000.code.entity.equipment.weapon;

namespace y1000.code.entity.player
{
    public partial class Player : Node2D
    {
        public float gravity = 0;

        public Point Coordinate { get; set; }

        public long Id {get;}

        private readonly long id;

        public bool IsMale {get; set;}

        public string? ChestArmor {get; set;}

        public string? Hat {get; set;}

        public string? Trousers { get; set; }

        public string? Weapon { get; set; }

        public IPlayerState PlayerState {get; set;}


        private readonly Queue<StateSegment> stateSegments = new();

        public 

        public void HandleMessage() {

        }

    }
}