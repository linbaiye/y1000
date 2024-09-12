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
        _bgmCheckBox.Pressed += OnBgmCheckBoxChanged;
        _soundSlider = GetNode<HSlider>("SoundVolSlider");
        _soundCheckBox = GetNode<CheckBox>("SoundCheckBox");
        _soundSlider.DragEnded += OnSndSliderEnd;
        _soundSlider.ValueChanged += OnSndSliderValueChanged;
        _close = GetNode<Button>("Close");
        _close.Pressed += () => Visible = false;
        Visible = false;
    }

    private void OnBgmCheckBoxChanged()
    {
        _audioManager?.OnBgmCheckChanged(_bgmCheckBox.ButtonPressed);
    }
    
    private void OnSoundCheckboxChanged()
    {
        _audioManager?.OnSoundCheckChanged(_soundCheckBox.ButtonPressed);
    }

    public void BindAudioManager(AudioManager audioManager)
    {
        _audioManager = audioManager;
        _bgmCheckBox.ButtonPressed = _audioManager.BgmEnabled;
        _soundCheckBox.ButtonPressed = _audioManager.SoundEnabled;
        _bgmSlider.SetValueNoSignal(_audioManager.BgmVolume);
        _soundSlider.SetValueNoSignal(_audioManager.SoundVolume);
    }
    
    private void OnSndSliderValueChanged(double val)
    {
        _audioManager?.OnSoundVolumeChanged(_soundSlider.Value);
    }

    private void OnSndSliderEnd(bool changed)
    {
        if (!changed)
            return;
        _audioManager?.OnSoundVolumeChanged(_soundSlider.Value);
    }

    private void OnBgmSliderValueChanged(double val)
    {
        _audioManager?.OnBgmVolumeChanged(_bgmSlider.Value);
    }

    private void OnBgmSliderEnd(bool changed)
    {
        if (!changed)
            return;
        _audioManager?.OnBgmVolumeChanged(_bgmSlider.Value);
    }
}