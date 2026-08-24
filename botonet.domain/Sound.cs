namespace Botonet.Domain;

public sealed class Sound
{
    public Guid Id { get; }
    public string Name { get; }
    public string FilePath { get; }

    public Sound(Guid id, string name, string filePath)
    {
        Id = id;
        Name = name;
        FilePath = filePath;
    }
}
