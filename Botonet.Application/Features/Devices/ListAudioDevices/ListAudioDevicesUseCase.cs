using Botonet.Domain;

namespace Botonet.Application.Features.Devices.ListAudioDevices;

public sealed class ListAudioDevicesUseCase
{
    private readonly IAudioDeviceProvider _audioDeviceProvider;

    public ListAudioDevicesUseCase(IAudioDeviceProvider audioDeviceProvider) => _audioDeviceProvider = audioDeviceProvider;

    public IReadOnlyList<AudioDevice> Execute() => _audioDeviceProvider.GetOutputDevices();
}
