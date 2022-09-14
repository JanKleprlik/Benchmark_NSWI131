using BenchmarkDotNet.Attributes;
using NWav = NWaves.Transforms;

namespace FFTBenchmark.Benchmarks
{
    public class NWavesRealBenchmark : BenchmarkBase
    {
        protected override bool usesComplex => false;

        private NWav.RealFft _fft;
        
        private float[] realInput;
        private float[] realOut;
        private float[] imagOut;

        public override void Prepare()
        {
            base.Prepare();

            _fft = new NWav.RealFft(Size/2);

            realInput = new float[Size / 2];
            
            for (int idx = 0; idx < Size / 2; idx++)
                realInput[idx] = (float)dataDouble[idx * 2];
            
            realOut = new float[Size];
            imagOut = new float[Size];
        }

        [Benchmark]
        public void NWavesReal() => _fft.Direct(realInput, realOut, imagOut);

    }
}
