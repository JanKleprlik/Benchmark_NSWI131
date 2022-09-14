using BenchmarkDotNet.Attributes;
using NWav = NWaves.Transforms;

namespace FFTBenchmark.Benchmarks
{
    public class NWaves64Benchmark : BenchmarkBase
    {
        protected override bool usesComplex => false;

        private NWav.Fft64 _fft;
        private double[] real;
        private double[] imag;

        public override void Prepare()
        {
            base.Prepare();

            _fft = new NWav.Fft64(Size/2);
            
            imag = new double[Size/2];
            real = new double[Size/2];

            for (int idx = 0; idx < Size / 2; idx++)
            {
                real[idx] = dataDouble[idx * 2];
                imag[idx] = dataDouble[idx * 2 + 1];
            }
        }

        [Benchmark]
        public void NWaves64() => _fft.Direct(real, imag);

    }
}
