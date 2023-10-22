using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace test.cide
{
    public abstract partial class AbstractBodyPart : Sprite2D
    {
        private Func<int> spriteIndexGetter = () => -1;

        private Character character;

        protected SpriteContainer SpriteContainer {get; set;}

        protected abstract Texture2D GetTexture(int spriteNumber);

        protected abstract Vector2 GetPosition(int spriteNumber);

        public void SetIndexGetter(Func<int> func)
        {
            spriteIndexGetter = func;
        }

        protected Direction Direction {get {
                Character character = GetParent<Character>();
                return character.Direction;
            }
        }

        protected State State {get {
                Character character = GetParent<Character>();
                return character.State;
            }
        }

        public void SetCharacter(Character ch) {
            character = ch;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            Character character = GetParent<Character>();
            int n = spriteIndexGetter.Invoke();
            //GD.Print(character.State +  ", Picnumber " + n);
            if (n != -1) {
                Position = GetPosition(n);
                Texture = GetTexture(n);
            }
        }
    }
}