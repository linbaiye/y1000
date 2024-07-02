using System;
using System.Linq;
using Godot;
using NLog;
using y1000.Source.Event;

namespace y1000.Source.Control;

public partial class TradeInputWindow : NinePatchRect
{
	private Label _itemName;
	
	private TextureButton _confirmButton;
	private TextureButton _cancelButton;
	
	private LineEdit _input;
	
	private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

	private Action<bool>? _callback;
	private EventMediator? _eventMediator;
	
	private ITradeWindowItem? _item;
	
	public T? Item<T>() where T : ITradeWindowItem
	{
		return _item != null ? (T)_item : default;
	}
	
	public interface ITradeWindowItem
	{
		string Name { get; }
	}

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
	}

	public void Open(string name, Action<bool> callback)
	{
		Open(name, "", callback);
	}

	private void Open(string name, string number, Action<bool> callback)
	{
		_callback = callback;
		_itemName.Text = name;
		_input.Text = number;
		Visible = true;
		_input.GrabFocus();
	}
	
	public void Open(ITradeWindowItem item, long number, Action<bool> callback)
	{
		_callback = callback;
		_itemName.Text = item.Name;
		_input.Text = number != 0 ? number.ToString() : "";
		Visible = true;
		_input.GrabFocus();
		_item = item;
	}
	
	
	public void Open(string name, long number, Action<bool> callback)
	{
		Open(name, number.ToString(), callback);
	}

	private void OnCancelPressed()
	{
		_callback?.Invoke(false);
		Visible = false;
	}

	public string? ItemName => _itemName.Text;

	private string? ValidateInput()
	{
		if (!_input.Text.All(char.IsDigit))
		{
			return "请输入正确数量";
		}

		var i = long.Parse(_input.Text);
		if (i < 1 || i > 1000000)
		{
			return "请输入正确数量";
		}
		return null;
	}

	public long Number => ValidateInput() != null ? 0 : long.Parse(_input.Text);

	private void OnConfirmPressed()
	{
		var validation = ValidateInput();
		if (validation != null)
		{
			_eventMediator?.NotifyTextArea(validation);
		}
		else
		{
			_callback?.Invoke(true);
		}
		Visible = false;
	}
    
}