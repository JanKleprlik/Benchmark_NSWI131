using BenchmarkDotNet.Attributes;
using MN = MathNet.Numerics;

using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace FFTBenchmark.Benchmarks
{
    public class MathNetBenchmark : BenchmarkBase
    {
        protected override bool usesComplex => true;

        [Benchmark]
        public void MathNet() =>
            MN.IntegralTransforms.Fourier.Forward(dataComplex);
    }
}
