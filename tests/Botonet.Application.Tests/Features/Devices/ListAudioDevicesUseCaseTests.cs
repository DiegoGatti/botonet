using Botonet.Application.Features.Devices;
using Botonet.Application.Features.Devices.ListAudioDevices;
using Botonet.Domain;

namespace Botonet.Application.Tests.Features.Devices;

public sealed class ListAudioDevicesUseCaseTests
{
    private sealed class FakeAudioDeviceProvider : IAudioDeviceProvider
    {
        private readonly IReadOnlyList<AudioDevice> _devices;

        public FakeAudioDeviceProvider(IReadOnlyList<AudioDevice> devices) => _devices = devices;

        public IReadOnlyList<AudioDevice> GetOutputDevices() => _devices;
    }

    [Fact]
    public void Execute_ReturnsDevicesFromProvider()
    {
        // Arrange
        var devices = new[] { new AudioDevice("1", "device-1"), new AudioDevice("2", "device-2") };
        var audioProvider = new FakeAudioDeviceProvider(devices);
        var listAudioDevicesUseCase = new ListAudioDevicesUseCase(audioProvider);

        // Act
        var result = listAudioDevicesUseCase.Execute();

        // Assert
        Assert.Equal(devices, result);
    }
}
