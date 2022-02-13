using System.Text.Json.Serialization;

namespace DynamicDriving.BuildingBlocks.PerformanceTests;

public record Test(Guid Id, string Value);

[JsonSerializable(typeof(Test), GenerationMode = JsonSourceGenerationMode.Default)]
public partial class TestDefaultModeContext : JsonSerializerContext
{
}

[JsonSerializable(typeof(Test), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class TestMetadataModeContext : JsonSerializerContext
{
}

[JsonSerializable(typeof(Test), GenerationMode = JsonSourceGenerationMode.Serialization)]
public partial class TestSerializationModeContext : JsonSerializerContext
{
}
