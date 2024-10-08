using System;
using Godot;
using y1000.Source.Control.RightSide;
using y1000.Source.Control.RightSide.Inventory;
using y1000.Source.Event;
using y1000.Source.Networking;
using y1000.Source.Networking.Server;
using y1000.Source.Util;

namespace y1000.Source.Control.Buff
{
    public partial class BuffContainer : VBoxContainer
    {

        private double _time;
        private InventorySlotView _slotView;
        private Label _text;

        private EventMediator? _eventMediator;

        public override void _Ready()
        {
            _slotView = GetNode<InventorySlotView>("InventorySlot1");
            _slotView.OnMouseInputEvent += HandleMouseEvent;
            _text = GetNode<Label>("Text");
            _time = 0;
            Visible = false;
        }

        public void Initialize(EventMediator eventMediator)
        {
            _eventMediator = eventMediator;
        }

        private void HandleMouseEvent(object? sender, SlotMouseEvent mouseEvent)
        {
            if (mouseEvent.IsRightClick)
            {
                _eventMediator?.NotifyServer(ClientSimpleCommandEvent.CancelBuff);
            }
        }

        private string FormatText()
        {
            TimeSpan time = TimeSpan.FromSeconds((long)_time);
            if (time.Hours > 0)
            {
                return time.ToString(@"hh") + "h\n" + time.ToString(@"mm") + "m";
            }
            else if (time.Minutes > 0)
            {
                return time.ToString(@"mm") + "m";
            }
            else
            {
                return time.ToString(@"ss") + "s";
            }
        }

        private void SetTime(int seconds)
        {
            _time = seconds;
            _text.Text = FormatText();
        }

        public override void _Process(double delta)
        {
            if (!Visible)
                return;
            if (_time > 0)
                _time -= delta;
            _text.Text = FormatText();
        }


        public void Handle(UpdateBuffMessage message)
        {
            if (message.IsGain && message.Texture != null)
            {
                var text = message.Description != null ? message.Description.ReplaceBr() + "单击右键取消。" : "单击右键取消。";
                _slotView.PutTextureAndTooltip(message.Texture, text);
                SetTime(message.Seconds);
                Visible = true;
            }
            else if (message.IsFade)
            {
                Visible = false;
            }
        }
    }
}