using BenchmarkDotNet.Attributes;
using NWav = NWaves.Transforms;

namespace FFTBenchmark.Benchmarks
{
    public class NWaves64RealBenchmark : BenchmarkBase
    {
        protected override bool usesComplex => false;

        private NWav.RealFft64 _fft;

        private double[] realInput;
        private double[] realOut;
        private double[] imagOut;

        public override void Prepare()
        {
            base.Prepare();

            _fft = new NWav.RealFft64(Size/2);

            realInput= new double[Size/2];
            realOut = new double[Size];
            imagOut = new double[Size];

            for (int idx = 0; idx < Size / 2; idx++)
            {
                realInput[idx] = dataDouble[idx * 2];
            }
        }

        [Benchmark]
        public void NWaves64Real() => _fft.Direct(realInput, realOut, imagOut);

    }
}
