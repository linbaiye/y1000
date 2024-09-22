using System;
using NLog;
using y1000.Source.Character;
using y1000.Source.Character.Event;
using y1000.Source.Creature;
using y1000.Source.Input;

namespace y1000.Source.Assistant;

public class AutoMoveAssistant
{
    private readonly CharacterImpl _character;
    private readonly InputSampler _inputSampler;
    private const double Interval = 0.5f;
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private double _timer;

    private Direction? _current;

    private bool _mouseWithinWindow;

    public AutoMoveAssistant(CharacterImpl character,
        InputSampler inputSampler)
    {
        _inputSampler = inputSampler;
        _character = character;
        character.WhenCharacterUpdated += OnCharacterMoved;
        _timer = 0.5f;
        Enabled = false;
        _mouseWithinWindow = true;
    }


    private bool Enabled
    {
        get;
        set;
    }

    public void Toggle()
    {
        Enabled = !Enabled;
        if (Enabled)
        {
            DoMove();
        }
    }
    
    public void Process(double delta)
    {
        if (!Enabled)
        {
            return;
        }
        _timer -= delta;
        if (_timer > 0)
        {
            return;
        }
        _timer = Interval;
        DoMove();
    }


    public void MouseExitWindow() {
        _mouseWithinWindow = false;
    }

    public void MouseEnterWindow() {
        _mouseWithinWindow = true;
    }



    private RightMousePressedMotion CreateInput()
    {
        if (!_mouseWithinWindow && _current.HasValue)
        {
            return _inputSampler.SampleMoveInput(_current.Value);
        }
        else
        {
            return _inputSampler.SampleMoveInput(_character.WrappedPlayer().GetLocalMousePosition());
        }
    }


    private void DoMove()
    {
        if (!Enabled)
        {
            return;
        }
        RightMousePressedMotion input = CreateInput();
        if (!_character.CanMoveOneUnit(input.Direction))
        {
            return;
        }
        _current = input.Direction;
        if (_character.Idle)
        {
            _character.HandleInput(input);
            if (_character.Moving)
            {
                _character.HandleInput(CreateInput());
            }
        }
        else if (_character.Moving)
        {
            _character.HandleInput(CreateInput());
        }
    }


    private void OnCharacterMoved(object? sender, EventArgs eventArgs)
    {
        if (eventArgs is not CharacterMoveEventArgs || sender is not CharacterImpl)
            return;
        DoMove();
    }
}