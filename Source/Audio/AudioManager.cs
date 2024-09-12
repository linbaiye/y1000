using System.Text.Json;
using System.Threading.Tasks;
using Godot;
using NLog;
using y1000.Source.Character;
using y1000.Source.Networking.Server;
using y1000.Source.Storage;

namespace y1000.Source.Audio;

public partial class AudioManager : Godot.Control
{
    
    private AudioStreamPlayer _bgmAudioPlayer;
    
    private static readonly ILogger LOGGER = LogManager.GetCurrentClassLogger();

    private readonly CreatureAudio[] _entitySoundPlayers = new CreatureAudio[4];

    private const string FileName = "audio";

    private FileStorage? _storage;

    private bool _dirty;

    private double _timer = 10;
    
    private class Settings
    {
        public bool BgmEnabled { get; set; }
        public double BgmVolume { get; set; }
        public bool SoundEnabled { get; set; }
        public double SoundVolume { get; set; }
    }

    private Settings _settings = new()
    {
        BgmEnabled = true,
        BgmVolume = 100,
        SoundEnabled = true,
        SoundVolume = 100,
    };

    public override void _Ready()
    {
        _bgmAudioPlayer = GetNode<AudioStreamPlayer>("BgmPlayer");
        _bgmAudioPlayer.Finished += ReplayBgm;
        for (int i = 0; i < _entitySoundPlayers.Length; i++)
        {
            _entitySoundPlayers[i] = GetNode<CreatureAudio>("SoundPlayer" + (i + 1));
        }
    }
    
    private async void ReplayBgm()
    {
        await Task.Delay(10000);
        _bgmAudioPlayer.Play();
    }
    
    public void OnSoundVolumeChanged(double v)
    {
        SoundVolume = v;
        foreach (var entitySoundPlayer in _entitySoundPlayers)
        {
            entitySoundPlayer.VolumeDb = (float)Mathf.LinearToDb(v / 100);
        }
        _dirty = true;
    }

    public override void _Process(double delta)
    {
        if (!_dirty)
        {
            return;
        }
        _timer -= delta;
        if (_timer <= 0)
        {
            Save();
            _timer = 10;
        }
    }
    
    private void Save()
    {
        _storage?.Save(FileName, JsonSerializer.Serialize(_settings));
    }

    public void OnBgmVolumeChanged(double v)
    {
        BgmVolume = v;
        _bgmAudioPlayer.VolumeDb = (float)Mathf.LinearToDb(v / 100);
        _dirty = true;
    }

    public void OnBgmCheckChanged(bool enabled)
    {
        BgmEnabled = enabled;
        _dirty = true;
    }

    
    public void OnSoundCheckChanged(bool enabled)
    {
        SoundEnabled = enabled;
        _dirty = true;
    }

    public bool BgmEnabled
    {
        get => _settings.BgmEnabled;
        private set => _settings.BgmEnabled = value;
    }

    public double BgmVolume
    {
        get => _settings.BgmVolume;
        private set => _settings.BgmVolume = value;
    }

    public bool SoundEnabled
    {
        get => _settings.SoundEnabled;
        private set => _settings.SoundEnabled = value;
    }

    public double SoundVolume
    {
        get => _settings.SoundVolume;
        private set => _settings.SoundVolume = value;
    }

    public void PlaySound(EntitySoundMessage message)
    {
        foreach (var t in _entitySoundPlayers)
        {
            if (!t.Playing)
            {
                t.PlaySound(message.Sound);
                break;
            }
        }
    }
    
    public void LoadAndPlayBackgroundMusic(string bgm)
    {
        var path = "res://assets/bgm/" + bgm + ".mp3";
        if (!FileAccess.FileExists(path))
        {
            path = "res://assets/bgm/" + bgm + ".wav";
        }
        if (FileAccess.FileExists(path))
        {
            var streamWav = ResourceLoader.Load<AudioStreamMP3>(path);
            _bgmAudioPlayer.Stop();
            _bgmAudioPlayer.Stream = streamWav;
            _bgmAudioPlayer.Play();
        }
    }

    private void Restore(string bgm)
    {
        _bgmAudioPlayer.VolumeDb = (float)Mathf.LinearToDb(_settings.BgmVolume / 100);
        foreach (var entitySoundPlayer in _entitySoundPlayers)
        {
            entitySoundPlayer.VolumeDb = (float)Mathf.LinearToDb(_settings.SoundVolume / 100);
        }
        LoadAndPlayBackgroundMusic(bgm);
    }

    public override void _Notification(int what)
    {
        if (what == NotificationWMCloseRequest && _dirty)
        {
            Save();
        }
    }

    public void Restore(CharacterImpl character, string bgm)
    {
        _storage =  new FileStorage(character.EntityName);
        var content = _storage.Load(FileName);
        if (content != null)
        {
            var tmp = JsonSerializer.Deserialize<Settings>(content);
            if (tmp != null)
            {
                _settings = tmp;
            }
        }
        Restore(bgm);
    }
    
}