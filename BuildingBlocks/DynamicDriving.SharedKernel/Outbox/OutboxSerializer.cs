using System.Text.Json;

namespace DynamicDriving.SharedKernel.Outbox;

public static class OutboxSerializer
{
    public static async Task<T> DeserializeAsync<T>(OutboxMessage outboxMessage)
    {
        Guards.ThrowIfNull(outboxMessage);

        var type = typeof(T).Assembly.GetType(outboxMessage.Type) ?? 
                   throw new InvalidOperationException($"Could not find type {outboxMessage.Type}");

        using var stream = new MemoryStream();
        await using var writer = new Utf8JsonWriter(stream);

        writer.WriteRawValue(outboxMessage.Data);
        await writer.FlushAsync().ConfigureAwait(false);
        stream.Position = 0;

        var result = (T)(await JsonSerializer.DeserializeAsync(stream, type).ConfigureAwait(false))!;

        return result;
    }
}
