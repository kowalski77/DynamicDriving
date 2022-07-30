namespace DynamicDriving.SharedKernel.Logging;

public class SeqSettings
{
    public string? Host { get; init; }

    public string? Port { get; init; }

    public string? ServerUrl => $"http://{this.Host}:{this.Port}";
}
