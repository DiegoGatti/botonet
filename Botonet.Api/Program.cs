using Botonet.Application.Features.Audio;
using Botonet.Application.Features.Audio.PlaySound;
using Botonet.Application.Features.Devices;
using Botonet.Application.Features.Devices.ListAudioDevices;
using Botonet.Domain;
using Botonet.Infrastructure.Audio;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddTransient<IAudioPlayer, NAudioPlayer>();
builder.Services.AddTransient<PlaySoundUseCase>();
builder.Services.AddTransient<IAudioDeviceProvider, NAudioDeviceProvider>();
builder.Services.AddTransient<ListAudioDevicesUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapPost("/api/sounds/test/play", async (PlaySoundUseCase playSoundUseCase, IWebHostEnvironment environment) =>
{
    var filePath = Path.Combine(environment.ContentRootPath, "Sounds", "test.mp3");
    var sound = new Sound(Guid.NewGuid(), "Test sound", filePath);

    await playSoundUseCase.ExecuteAsync(sound);

    return Results.NoContent();
});

app.MapGet("/api/devices", (ListAudioDevicesUseCase listAudioDevices) =>
{
    var devices = listAudioDevices.Execute();
    return Results.Ok(devices);
});

app.Run();
