using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FFTBenchmark.Benchmarks
{
    public class DummyRecursiveBenchmark : BenchmarkBase
    {
        protected override bool usesComplex => true;

		//[Benchmark(Baseline = true)]
		public void DummyRecursive() => DFT_Recurr(dataComplex);


		/// <summary>
		/// Recursive DFT - out of place
		/// Not working
		/// </summary>
		public Complex[] DFT_Recurr(Complex[] data)
		{
			int n = data.Length;

			if (n == 1)
				return new Complex[] { 1, data[0] };

			Complex[] even = new Complex[n / 2];
			Complex[] odd = new Complex[n / 2];

			for (int i = 0; i < n / 2; i++)
			{
				even[i] = data[i * 2];
				odd[i] = data[i * 2 + 1];
			}

			even = DFT_Recurr(even);
			odd = DFT_Recurr(odd);

			Complex[] bin = new Complex[n];

			Complex[] w = new Complex[n];
			for (int i = 0; i < n; i++)
			{
				double alpha = 2.0 * Math.PI * (double)i / (double)n;
				w[i] = new Complex(Math.Cos(alpha), Math.Sin(alpha));
			}

			for (int i = 0; i < n / 2; i++)
			{
				bin[i] = even[i] + w[i] * odd[i];
				bin[i + n / 2] = even[i] - w[i] * odd[i];
			}

			return bin;
		}

	}
}
