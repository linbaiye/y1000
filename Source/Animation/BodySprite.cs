using Godot;
using NLog;
using y1000.Source.Creature;

namespace y1000.Source.Animation
{
	public partial class BodySprite : AbstractPartSprite
	{
		protected override OffsetTexture OffsetTexture => GetParent<IBody>().BodyOffsetTexture;

		private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

		private Panel _panel;
		private Label _label;
		private Label _label2;

		public override void _Ready()
		{
			_panel = GetNode<Panel>("Area");
			_panel.MouseEntered += MouseEnteredArea;
			_panel.MouseExited += MouseExitedArea;
			_label = GetNode<Label>("Label");
			_label.Visible = false;
			_label2 = GetNode<Label>("Label2");
			_label2.Visible = false;
			UpdateColor();
			UpdateGuild();
		}

		public void SetName(string name)
		{
			_label.Text = name;
		}

		public void UpdateColor()
		{
			var color = GetParent<IBody>().NameColor;
			_label.AddThemeColorOverride("font_color", new Color(color));
		}

		public void UpdateGuild()
		{
			var guildName = GetParent<IBody>().GuildName;
			if (!string.IsNullOrEmpty(guildName))
			{
				_label2.Text = guildName;
			}
			else
			{
				_label2.Text = "";
			}
		}

		private void MouseEnteredArea()
		{
			_label.Visible = true;
			if (!string.IsNullOrEmpty(_label2.Text))
				_label2.Visible = true;
		}
		
		private void MouseExitedArea()
		{
			_label.Visible = false;
			_label2.Visible = false;
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
