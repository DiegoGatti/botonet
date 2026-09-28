using Botonet.Application.Features.Audio;
using Botonet.Application.Features.Audio.PlaySound;
using Botonet.Domain;

namespace Botonet.Application.Tests;

public class PlaySoundUseCaseTests
{
    private sealed class FakeAudioPlayer : IAudioPlayer
    {
        public string? PlayedFilePath { get; private set; }

        public Task PlayAsync(string filePath)
        {
            this.PlayedFilePath = filePath;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task ExecuteAsync_PassesSoundFilePathToAudioPlayer()
    {
        // Arrange
        var audioPlayer = new FakeAudioPlayer();
        var useCase = new PlaySoundUseCase(audioPlayer);
        var sound = new Sound(new Guid(), "Test sound", "sounds/test.mp3");
        // Act
        await useCase.ExecuteAsync(sound);
        // Assert
        Assert.Equal(sound.FilePath, audioPlayer.PlayedFilePath);
    }
}