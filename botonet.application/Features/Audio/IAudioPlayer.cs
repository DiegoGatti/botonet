namespace Botonet.Application.Features.Audio;

public interface IAudioPlayer
{
    Task PlayAsync(string filePath);
}
