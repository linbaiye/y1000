using System.Threading.Tasks;
using Godot;
using y1000.Source.Networking.Server;

namespace y1000.Source.Audio;

public partial class AudioManager : Godot.Control
{
    
    private AudioStreamPlayer _bgmAudioPlayer;

    private readonly CreatureAudio[] _entitySoundPlayers = new CreatureAudio[4];

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

    public void OnBgmVolumeChanged(double v)
    {
        _bgmAudioPlayer.VolumeDb = (float)Mathf.LinearToDb(v);
    }


    public void PlaySound(EntitySoundMessage message)
    {
        for (int i = 0; i < _entitySoundPlayers.Length; i++)
        {
            if (!_entitySoundPlayers[i].Playing)
            {
                _entitySoundPlayers[i].PlaySound(message.Sound);
                return;
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
}