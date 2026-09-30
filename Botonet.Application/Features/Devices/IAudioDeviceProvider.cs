using Botonet.Domain;

namespace Botonet.Application.Features.Devices;

public interface IAudioDeviceProvider
{
    IReadOnlyList<AudioDevice> GetOutputDevices();
}
