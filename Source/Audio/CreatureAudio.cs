using Godot;

namespace y1000.Source.Audio;

public partial class CreatureAudio : AudioStreamPlayer
{
    public void PlaySoundEffect()
    {
        var streamWav = ResourceLoader.Load<AudioStreamWav>("res://assets/sound/5804.wav");
        Stream = streamWav;
        Play();
    }
}