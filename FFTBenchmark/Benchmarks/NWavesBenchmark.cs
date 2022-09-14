using BenchmarkDotNet.Attributes;
using NWav = NWaves.Transforms;

namespace FFTBenchmark.Benchmarks
{
    public class NWavesBenchmark : BenchmarkBase
    {
        protected override bool usesComplex => false;

        private NWav.Fft _fft;
        private float[] real;
        private float[] imag;
        
        public override void Prepare()
        {
            base.Prepare();
            
            _fft = new NWav.Fft(Size);

            real = new float[Size];
            imag = new float[Size];
            
            for (int i = 0; i < dataComplex.Length; i++)
            {
                real[i] = (float)dataComplex[i].Real;
                imag[i] = (float)dataComplex[i].Imaginary;
            }
        }

        [Benchmark]
        public void NWaves() => _fft.Direct(real, imag);

    }
}
