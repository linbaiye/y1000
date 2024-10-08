using Godot;

namespace y1000.Source.Audio;

public partial class CreatureAudio : AudioStreamPlayer
{
    public void PlaySound(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        var path = "res://assets/sound/" + name + ".wav";
        if (ResourceLoader.Exists(path))
        {
            var streamWav = ResourceLoader.Load<AudioStreamWav>(path);
            Stream = streamWav;
            Play();
        }
    }
}