using System.Text.Json;

namespace DynamicDriving.SharedKernel.Outbox;

public static class OutboxSerializer
{
    public static async Task<object> DeserializeAsync(OutboxMessage outboxMessage)
    {
        Guards.ThrowIfNull(outboxMessage);

        var type = Type.GetType(outboxMessage.Type);
        if(type is null)
        {
            throw new InvalidOperationException($"Could not find type {outboxMessage.Type}");
        }

        using var stream = new MemoryStream();
        await using var writer = new Utf8JsonWriter(stream);

        writer.WriteRawValue(outboxMessage.Data);
        await writer.FlushAsync().ConfigureAwait(false);
        stream.Position = 0;

        var result = (await JsonSerializer.DeserializeAsync(stream, type).ConfigureAwait(false))!;

        return result;
    }
}
