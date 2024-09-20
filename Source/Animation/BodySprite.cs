using System;
using Godot;
using NLog;
using y1000.Source.Creature;
using y1000.Source.Util;

namespace y1000.Source.Animation
{
	public partial class BodySprite : AbstractPartSprite
	{
		protected override OffsetTexture OffsetTexture => GetParent<IBody>().BodyOffsetTexture;

		private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

		private Panel _panel;
		private Label _label;

		public override void _Ready()
		{
			_panel = GetNode<Panel>("Area");
			_panel.MouseEntered += MouseEnteredArea;
			_panel.MouseExited += MouseExitedArea;
			_label = GetNode<Label>("Label");
			_label.Visible = false;
		}

		public void SetName(string name)
		{
			_label.Text = name;
		}

		public void SetNameColor(int color)
		{
			_label.AddThemeColorOverride("font_color", new Color(color.ToString("X6")));
		}

		private void MouseEnteredArea()
		{
			_label.Visible = true;
		}
		
		private void MouseExitedArea()
		{
			_label.Visible = false;
		}

		public Panel Area => _panel;

		public Label Label => _label;
		
		public override void _Process(double delta)
		{
			var texture = OffsetTexture;
			Offset = texture.Offset;
			Texture = texture.Texture;
			_panel.Position = texture.Offset;
			_panel.Size = texture.OriginalSize;
		}
	}
}
