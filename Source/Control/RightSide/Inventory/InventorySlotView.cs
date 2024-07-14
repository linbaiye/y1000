using System;
using System.Text.RegularExpressions;
using Godot;
using NLog;

namespace y1000.Source.Control.RightSide.Inventory;


public partial class InventorySlotView : Panel
{
	private int _number;
	
	private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
	public event EventHandler<SlotEvent>? OnInputEvent;
	

	private bool _mouseHovered;

	private TextureRect _textureRect;
	
	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			HandleMouseEvent(eventMouseButton);
		}
	}

	public override void _ShortcutInput(InputEvent @event)
	{
		if (@event is InputEventKey && _mouseHovered)
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
				OnInputEvent?.Invoke(this, new SlotEvent(SlotEvent.Type.MOUSE_LEFT_DOUBLE_CLICK));
			}
			// else if (eventMouse.Pressed)
			// {
			// 	OnInputEvent?.Invoke(this, new SlotEvent(SlotEvent.Type.MOUSE_LEFT_CLICK));
			// }
			else if (eventMouse.IsReleased())
			{
				OnInputEvent?.Invoke(this, new SlotEvent(SlotEvent.Type.MOUSE_LEFT_RELEASE));
			}
		}
		else if (eventMouse.ButtonMask == MouseButtonMask.Right)
		{
			OnInputEvent?.Invoke(this, new SlotEvent(SlotEvent.Type.MOUSE_RIGHT_CLICK));
		}
	}

	private void OnMouseEntered()
	{
		_mouseHovered = true;
		OnInputEvent?.Invoke(this, new SlotEvent(SlotEvent.Type.MOUSE_ENTERED));
	}
	
	private void OnMouseExited()
	{
		_mouseHovered = false;
		OnInputEvent?.Invoke(this, new SlotEvent(SlotEvent.Type.MOUSE_GONE));
	}


	public override void _Ready()
	{
		ParseNumber();
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
		_textureRect = GetNode<TextureRect>("CenterContainer/TextureRect");
	}


	private void ParseNumber()
	{
		var str = Regex.Match(Name, @"\d+").Value;
		_number = int.Parse(str);
	}

	public int Number => _number;

	public void PutTexture(Texture2D texture2D)
	{
		_textureRect.Texture = texture2D;
	}

	public void PutTextureAndTooltip(Texture2D texture2D, string tip)
	{
		_textureRect.Texture = texture2D;
		TooltipText = tip;
	}

	public void Clear()
	{
		_textureRect.Texture = null;
		TooltipText = "";
	}

	public bool IsEmpty => _textureRect.Texture != null;
}
