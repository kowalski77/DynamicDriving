﻿using System.Reflection;
using System.Text.Json;

namespace DynamicDriving.SharedKernel.Outbox;

public static class OutboxSerializer
{
    public static T Deserialize<T>(OutboxMessage outboxMessage, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(outboxMessage);
        ArgumentNullException.ThrowIfNull(assembly);

        var type = assembly.GetType(outboxMessage.Type) ?? 
                   throw new InvalidOperationException($"Could not find type {outboxMessage.Type}");

        var result = (T)JsonSerializer.Deserialize(outboxMessage.Data, type)!;

        return result;
    }
}