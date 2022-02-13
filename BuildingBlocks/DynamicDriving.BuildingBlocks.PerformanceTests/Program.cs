using BenchmarkDotNet.Running;
using DynamicDriving.BuildingBlocks.PerformanceTests;

_ = BenchmarkRunner.Run<JsonSerializerContextModesBenchmark>();
