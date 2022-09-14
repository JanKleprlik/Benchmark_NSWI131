using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math.Transforms;
using Accord.Math;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace FFTBenchmark.Benchmarks
{
    public class AccordBenchmark : BenchmarkBase
    {
        protected override bool usesComplex => true;

        [Benchmark]
        public void Accord() =>
            FourierTransform2.FFT(dataComplex, FourierTransform.Direction.Forward);
    }
}
