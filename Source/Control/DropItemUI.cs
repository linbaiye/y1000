using System.Linq;
using Godot;
using NLog;
using y1000.Source.Control.Bottom;
using y1000.Source.Event;
using y1000.Source.Item;
using y1000.Source.Networking;
using y1000.Source.Util;

namespace y1000.Source.Control;

public partial class DropItemUI : NinePatchRect
{
	private Label _itemName;

	private TextureButton _confirmButton;
	private TextureButton _cancelButton;
	
	private LineEdit _input;
	
	private EventMediator? _eventMediator;

	private DragInventorySlotEvent? _currentEvent;

	private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

	public override void _Ready()
	{
		_itemName = GetNode<Label>("ItemName");
		_input = GetNode<LineEdit>("Input");
		_confirmButton = GetNode<TextureButton>("ConfirmButton");
		_cancelButton = GetNode<TextureButton>("CancelButton");
		_confirmButton.Pressed += OnConfirmPressed;
		_cancelButton.Pressed += OnCancelPressed;
		Visible = false;
	}

	public void BindEventMediator(EventMediator eventMediator)
	{
		_eventMediator = eventMediator;
		_eventMediator.SetComponent(this);
	}
	
	private bool MouseWithinCharacterDropRange(Vector2 globalMousePosition, Vector2I characterCoordinate)
	{
		var start = characterCoordinate.Move(-2, -3).ToPosition();
		var end = characterCoordinate.Move(2, 3).ToPosition();
		return start.X <= globalMousePosition.X && end.X >= globalMousePosition.X && 
			   start.Y <= globalMousePosition.Y && end.Y >= globalMousePosition.Y;
	}

	public void OnDropItem(DragInventorySlotEvent dragInventorySlotEvent, Vector2 mousePosition, Vector2I characterCoordinate)
	{
		if (MouseWithinCharacterDropRange(mousePosition, characterCoordinate))
		{
			Visible = true;
			var characterItem = dragInventorySlotEvent.Item;
			_itemName.Text = characterItem.ItemName;
			_currentEvent = dragInventorySlotEvent;
			_currentEvent.AtPosition = mousePosition;
			if (characterItem is not CharacterStackItem)
			{
				_input.Text = "1";
				_input.Editable = false;
			}
			else
			{
				_input.Editable = true;
				_input.GrabFocus();
			}
		}
		else
		{
			_eventMediator?.NotifyTextArea("距离过远");
			_currentEvent = null;
		}
	}

	private string? ValidateInput()
	{
		if (!_input.Text.All(char.IsDigit))
		{
			return "请输入正确数量";
		}

		if (_currentEvent?.Item is not CharacterStackItem stackItem)
		{
			return null;
		}
		var number = int.Parse(_input.Text);
		if (number <= 0)
		{
			return "请输入正确数量";
		}

		if (number > stackItem.Number)
		{
			return "没有足够数量";
		}

		return null;
	}

	private void OnCancelPressed()
	{
		ClearAndHide();
	}

	private void ClearAndHide()
	{
		_input.Text = "";
		Visible = false;
	}
	

	private void OnConfirmPressed()
	{
		if (_currentEvent == null)
		{
			return;
		}
		var validation = ValidateInput();
		if (validation != null)
		{
			_eventMediator?.NotifyUiEvent(new TextEvent(validation));
		}
		else
		{
			 _eventMediator?.NotifyServer(new ClientDropItemEvent(_currentEvent.Slot,  int.Parse(_input.Text), _currentEvent.AtPosition,
				 _currentEvent.AtPosition.Snapped(VectorUtil.TileSize).ToCoordinate()));
		}
		ClearAndHide();
	}
	
}
