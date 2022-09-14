using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FFTBenchmark.Benchmarks
{
    public class DummyForLoopBenchmark : BenchmarkBase
    {
        protected override bool usesComplex => true;

        //[Benchmark]
        public void DummyForLoop() => DFT(dataComplex);

        /// <summary>
        /// forloop DFT - in place
        /// </summary>
        public static void DFT(Complex[] data)
		{
			int n = data.Length;
			double alpha;
			Complex[] bin = new Complex[n];

			for (int i = 0; i < n ; i++) //for audio, only one half is needed
			{
				alpha = 2.0 * Math.PI * i / (double)n;

				for (int j = 0; j < n; j++)
				{
					bin[i] += new Complex(data[j].Real * Math.Cos(j * alpha) - data[j].Imaginary * Math.Sin(j * alpha),
										data[j].Real * Math.Cos(j * alpha) + data[j].Imaginary * Math.Sin(j * alpha));
				}
			}

			for (int i = 0; i < n / 2; i++)
			{
				data[i] = new Complex(bin[i].Real / n, bin[i].Imaginary / n); //divide by n to scale 
			}
		}
	}
}
