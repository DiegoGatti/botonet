using Botonet.Application.Features.Devices;
using Botonet.Domain;
using NAudio.CoreAudioApi;

namespace Botonet.Infrastructure.Audio;

public sealed class NAudioDeviceProvider : IAudioDeviceProvider
{
    public IReadOnlyList<AudioDevice> GetOutputDevices()
    {
        using var enumerator = new MMDeviceEnumerator();
        
        return enumerator
            .EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
            .Select(device => new AudioDevice(device.ID, device.FriendlyName))
            .ToArray();
    }
}
