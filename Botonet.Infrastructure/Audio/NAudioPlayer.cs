using Botonet.Application.Features.Audio;
using NAudio.Wave;

namespace Botonet.Infrastructure.Audio;

public sealed class NAudioPlayer : IAudioPlayer
{
    public async Task PlayAsync(string filePath)
    {
        using var audioFile = new AudioFileReader(filePath);
        using var player = new WasapiPlayerBuilder().Build();
        player.Init(audioFile);
        player.Play();

        while (player.PlaybackState == PlaybackState.Playing)
        {
            await Task.Delay(100);
        }
    }
}
