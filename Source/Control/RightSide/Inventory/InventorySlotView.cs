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

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			HandleMouseEvent(eventMouseButton);
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
			else if (eventMouse.Pressed)
			{
				OnInputEvent?.Invoke(this, new SlotEvent(SlotEvent.Type.MOUSE_LEFT_CLICK));
			}
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

	public override void _Ready()
	{
		ParseNumber();
		MouseEntered += () => OnInputEvent?.Invoke(this, new SlotEvent(SlotEvent.Type.MOUSE_ENTERED));
		MouseExited += () => OnInputEvent?.Invoke(this, new SlotEvent(SlotEvent.Type.MOUSE_GONE));
	}


	private void ParseNumber()
	{
		var str = Regex.Match(Name, @"\d+").Value;
		_number = int.Parse(str);
	}

	public int Number => _number;

	public void PutItem(Texture2D texture2D)
	{
		GetNode<TextureRect>("CenterContainer/TextureRect").Texture = texture2D;
	}

	public void ClearTexture()
	{
		GetNode<TextureRect>("CenterContainer/TextureRect").Texture = null;
	}
}
