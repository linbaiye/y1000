using System;
using System.Text.RegularExpressions;
using Godot;

namespace y1000.Source.Control.RightSide.Book;

public partial class BookPage : TextureButton
{
    private int _number;

    private Action<BookPage>? _onClickedAction;
    
    public override void _Ready()
    {
        ParseNumber();
        FocusMode = FocusModeEnum.Click;
    }

    public void SetCallback(Action<BookPage> ca)
    {
        _onClickedAction = ca;
    }

    public int Number => _number;

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.IsPressed() &&
            eventMouseButton.ButtonIndex == MouseButton.Left)
        {
            _onClickedAction?.Invoke(this);
        }
    }
    private void ParseNumber()
    {
        var str = Regex.Match(Name, @"\d+").Value;
        _number = int.Parse(str);
    }
}