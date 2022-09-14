using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;


var config = DefaultConfig.Instance
    .AddHardwareCounters(
        HardwareCounter.BranchInstructions,
        HardwareCounter.BranchMispredictions,
        HardwareCounter.LlcReference
        )
    .AddDiagnoser(MemoryDiagnoser.Default)
    .AddJob(
        Job.MediumRun.WithId("Medium")
        //.WithOutlierMode(Perfolizer.Mathematics.OutlierDetection.OutlierMode.DontRemove)
        //.WithGcServer(true)
        )
    .WithOption(ConfigOptions.JoinSummary, true)
    .WithSummaryStyle(SummaryStyle.Default);

BenchmarkRunner.Run(typeof(Program).Assembly, config);
