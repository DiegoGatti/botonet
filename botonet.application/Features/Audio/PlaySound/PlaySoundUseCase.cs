using Botonet.Domain;

namespace Botonet.Application.Features.Audio.PlaySound;

public sealed class PlaySoundUseCase
{
    private readonly IAudioPlayer _audioPlayer;

    public PlaySoundUseCase(IAudioPlayer audioPlayer)
    {
        _audioPlayer = audioPlayer;
    }

    public Task ExecuteAsync(Sound sound)
    {
        return _audioPlayer.PlayAsync(sound.FilePath);
    }
}