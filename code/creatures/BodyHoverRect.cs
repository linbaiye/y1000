using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace y1000.code.creatures
{
    public partial class BodyHoverRect : TextureRect
    {
        private Label? label;

        private bool hide;

        public override void _Ready()
        {
            MouseEntered += OnMouseEntered;
            MouseExited += OnMouseEixted;
            label = GetNode<Label>("Label");
            label.Hide();
            label.Text = "测试千年";
            hide = true;
        }

        public void OnMouseEntered()
        {
            hide = false;
            label?.Show();
        }

        public Rect2I HoverRect()
        {
            var pos = GetScreenPosition();
            return new Rect2I((int)pos.X, (int)pos.Y, (int)Size.X, (int)Size.Y);
        }


        public void OnTextureChanged(Vector2 offset, int textureWidth, int textureHeight)
        {
            if (hide)
            {
                Position = offset;
                Size = new(textureWidth, textureHeight);
            }
        }

        public void OnMouseEixted()
        {
            hide = true;
            label?.Hide();
        }
    }
}