using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace FFTBenchmark.Benchmarks
{
    public abstract class BenchmarkBase
    {


        protected Complex[] dataComplex;
        protected double[] dataDouble;
        protected abstract bool usesComplex { get; }

        //[Params(1024, 2048, 4096, 16384)]
        [Params(16384, 65536, 33554432)]
        //[Params(16384)]
        public int Size;
        public double Frequency = 32;
        public double SamplingRate = 128;
        public double Amplitude = 128;


        [GlobalSetup]
        public virtual void Prepare()
        {

            dataDouble = DataGenerator.Sine(Size, SamplingRate, Frequency, Amplitude);
            dataComplex = new Complex[dataDouble.Length / 2];
            
            if (usesComplex)
            {
                Size /= 2;
                for (int i = 0; i < dataDouble.Length / 2; i++)
                {
                    dataComplex[i] = new Complex(dataDouble[i * 2], dataDouble[i * 2 + 1]);
                }
            }
        }
    }
}