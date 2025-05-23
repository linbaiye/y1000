using System;
using System.Text.RegularExpressions;
using Godot;
using NLog;
using y1000.Source.Util;

namespace y1000.Source.Control.RightSide.Inventory;


public partial class InventorySlotView : Panel
{
	private int _number;
	
	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
	public event EventHandler<SlotMouseEvent>? OnMouseInputEvent;
	public event EventHandler<SlotKeyEvent>? OnKeyboardEvent;

	private bool _mouseHovered;

	private TextureRect _textureRect;

	private RichTextLabel _label;
	
	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			HandleMouseEvent(eventMouseButton);
		}
	}

	public override void _ShortcutInput(InputEvent @event)
	{
		if (@event is not InputEventKey eventKey)
			return;
		if (_mouseHovered && eventKey.IsPressed() && eventKey.Keycode != Key.Enter)
		{
			OnKeyboardEvent?.Invoke(this, new SlotKeyEvent(eventKey.Keycode));
		}
		if (eventKey.Keycode != Key.Enter && _mouseHovered)
		{
			AcceptEvent();
		}
	}


	private void HandleMouseEvent(InputEventMouseButton eventMouse)
	{
		if (eventMouse.ButtonIndex == MouseButton.Left)
		{
			if (eventMouse.DoubleClick)
			{
				OnMouseInputEvent?.Invoke(this, new SlotMouseEvent(SlotMouseEvent.Type.MOUSE_LEFT_DOUBLE_CLICK));
			}
			else if (eventMouse.Pressed)
			{
				OnMouseInputEvent?.Invoke(this, new SlotMouseEvent(SlotMouseEvent.Type.MOUSE_LEFT_CLICK));
			}
			else if (eventMouse.IsReleased())
			{
				OnMouseInputEvent?.Invoke(this, new SlotMouseEvent(SlotMouseEvent.Type.MOUSE_LEFT_RELEASE));
			}
		}
		else if (eventMouse.ButtonMask == MouseButtonMask.Right)
		{
			OnMouseInputEvent?.Invoke(this, new SlotMouseEvent(SlotMouseEvent.Type.MOUSE_RIGHT_CLICK));
		}
	}

	private void OnMouseEntered()
	{
		_mouseHovered = true;
		OnMouseInputEvent?.Invoke(this, new SlotMouseEvent(SlotMouseEvent.Type.MOUSE_ENTERED));
	}
	
	private void OnMouseExited()
	{
		_mouseHovered = false;
		OnMouseInputEvent?.Invoke(this, new SlotMouseEvent(SlotMouseEvent.Type.MOUSE_GONE));
	}


	public override void _Ready()
	{
		ParseNumber();
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		_textureRect = GetNode<TextureRect>("CenterContainer/TextureRect");
		_label = GetNode<RichTextLabel>("Label");
	}


	private void ParseNumber()
	{
		var str = Regex.Match(Name, @"\d+").Value;
		_number = int.Parse(str);
	}

	public int Number => _number;

	public void PutTexture(Texture2D texture2D, int color = 0)
	{
		PutTextureAndTooltip(texture2D, "", color);
	}

	public void PutText(string text)
	{
		_label.Text = "[b]" + text;
	}

	public void ClearText()
	{
		_label.Text = "";
	}

	public void PutTextureAndTooltip(Texture2D texture2D, string tip, int color = 0)
	{
		_textureRect.Material = null;
		_textureRect.Texture = texture2D;
		TooltipText = tip;
		if (color != 0)
		{
			_textureRect.Material = DyeShader.CreateShaderMaterial(color);
		}
	}

	public Texture2D Texture => _textureRect.Texture;

	public void ClearTextureAndTip()
	{
		_textureRect.Texture = null;
		TooltipText = "";
	}

	public bool IsEmpty => _textureRect.Texture != null;
	
}
