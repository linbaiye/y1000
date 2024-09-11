using Godot;
using NLog;
using y1000.Source.Audio;

namespace y1000.Source.Control.System;

public partial class SystemSettings : NinePatchRect
{
    private CheckBox _bgmCheckBox;
    private HSlider _bgmSlider;
    private CheckBox _soundCheckBox;
    private HSlider _soundSlider;
    private CheckBox _shoutCheckBox;
    private CheckBox _guildCheckBox;
    private CheckBox _speakCheckBox;
    private Button _close;
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();
    private AudioManager? _audioManager;
    public override void _Ready()
    {
        _bgmSlider = GetNode<HSlider>("BgmVolSlider");
        _bgmSlider.DragEnded += OnBgmSliderEnd;
        _bgmSlider.ValueChanged += OnBgmSliderValueChanged;
        _bgmCheckBox = GetNode<CheckBox>("BgmCheckBox");
        _soundSlider = GetNode<HSlider>("SoundVolSlider");
        _soundCheckBox = GetNode<CheckBox>("SoundCheckBox");
        _close = GetNode<Button>("Close");
        _close.Pressed += () => Visible = false;
        Visible = false;
    }

    public void BindAudioManager(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    private void OnBgmSliderValueChanged(double val)
    {
        LOGGER.Debug("Value {0}.", _bgmSlider.Value);
        _audioManager?.OnBgmVolumeChanged(_bgmSlider.Value);
    }

    private void OnBgmSliderEnd(bool changed)
    {
        if (!changed)
            return;
        LOGGER.Debug("Value {0}.", _bgmSlider.Value);
        _audioManager?.OnBgmVolumeChanged(_bgmSlider.Value);
    }
}