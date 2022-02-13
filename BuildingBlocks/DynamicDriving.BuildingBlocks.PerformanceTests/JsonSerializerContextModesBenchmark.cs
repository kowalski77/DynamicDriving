using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace DynamicDriving.BuildingBlocks.PerformanceTests;

[MemoryDiagnoser]
public class JsonSerializerContextModesBenchmark
{
    private readonly Test testInstance = new(Guid.NewGuid(), "value1");

    // Default
    [Benchmark]
    public string DefaultMode()
    {
        return JsonSerializer.Serialize(this.testInstance, typeof(Test), TestDefaultModeContext.Default);
    }

    [Benchmark]
    public byte[] DefaultModeToBytes()
    {
        return JsonSerializer.SerializeToUtf8Bytes(this.testInstance, TestDefaultModeContext.Default.Test);
    }

    // Metadata
    [Benchmark]
    public string MetadataMode()
    {
        return JsonSerializer.Serialize(this.testInstance, typeof(Test), TestMetadataModeContext.Default);
    }

    [Benchmark]
    public byte[] MetadataModeToBytes()
    {
        return JsonSerializer.SerializeToUtf8Bytes(this.testInstance, TestMetadataModeContext.Default.Test);
    }

    // Serialization
    [Benchmark]
    public string SerializationMode()
    {
        return JsonSerializer.Serialize(this.testInstance, typeof(Test), TestSerializationModeContext.Default);
    }

    [Benchmark]
    public byte[] SerializationModeToBytes()
    {
        return JsonSerializer.SerializeToUtf8Bytes(this.testInstance, TestSerializationModeContext.Default.Test);
    }
}
