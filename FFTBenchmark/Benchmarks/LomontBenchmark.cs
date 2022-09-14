using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFTBenchmark.Benchmarks
{
    
    public class LomontBenchmark : BenchmarkBase
    {
        protected override bool usesComplex => false;

        [Benchmark(Baseline = true)]
        public void Lomont() => FFT(dataDouble);

		/// <summary>
		/// FFT specialized for audio
		/// Inspired by LomontFFT <see cref="https://www.lomont.org/software/misc/fft/LomontFFT.cs"/> and classic wikipedia implementation <see cref="https://en.wikipedia.org/wiki/Cooley%E2%80%93Tukey_FFT_algorithm"/>.
		/// Data are stored as real and imaginary doubles alternating.
		/// Data length must be a power of two.
		/// </summary>
		/// <param name="data">Complex valued data stored as doubles.
		/// Alternating between real and imaginary parts.</param>
		/// <exception cref="ArgumentException">data length is not power of two</exception>
		public void FFT(double[] data, bool normalize = false)
		{
			int n = data.Length;
			if (!LomontHelpers.IsPowOfTwo(n))
				throw new ArgumentException($"Data length: {n} is not power of two.");

			n /= 2; //data are represented as 1 double for Real part && 1 double for Imaginary part

			LomontHelpers.BitReverse(data);

			int max = 1;
			while (n > max)
			{
				int step = 2 * max;
				//helper variables for Real and Img separate computations
				double omegaReal = 1;
				double omegaImg = 0;
				double omegaCoefReal = Math.Cos(Math.PI / max);
				double omegaCoefImg = Math.Sin(Math.PI / max);
				for (int m = 0; m < step; m += 2) //2 because of Real + Img double
				{
					//2*n because we have double the amount of data (Re+Img)
					for (int k = m; k < 2 * n; k += 2 * step)
					{
						double tmpReal = omegaReal * data[k + step] - omegaImg * data[k + step + 1];
						double tmpImg = omegaImg * data[k + step] + omegaReal * data[k + step + 1];

						data[k + step] = data[k] - tmpReal;
						data[k + step + 1] = data[k + 1] - tmpImg;

						data[k] = data[k] + tmpReal;
						data[k + 1] = data[k + 1] + tmpImg;
					}
					//compute new omega
					double tmp = omegaReal;
					omegaReal = omegaReal * omegaCoefReal - omegaImg * omegaCoefImg;
					omegaImg = omegaImg * omegaCoefReal + tmp * omegaCoefImg;
				}

				max = step; //move logarithm loop
			}

			if (normalize)
				LomontHelpers.Normalize(data);
		}

	}

    static class LomontHelpers
    {
		public static void BitReverse(double[] data)
		{
			int n = data.Length / 2;
			int first = 0, second = 0;

			int top = n / 2;

			while (true)
			{
				//swapping real parts
				data[first + 2] = data[first + 2].Swap(ref data[second + n]);
				//swapping imaginary parts
				data[first + 3] = data[first + 3].Swap(ref data[second + n + 1]);

				if (first > second) //first and second met -> swap two more
				{
					//first
					//swapping real parts
					data[first] = data[first].Swap(ref data[second]);
					//swapping imaginary parts
					data[first + 1] = data[first + 1].Swap(ref data[second + 1]);

					//second
					//swapping real parts
					data[first + n + 2] = data[first + n + 2].Swap(ref data[second + n + 2]);
					//swapping imaginary parts
					data[first + n + 3] = data[first + n + 3].Swap(ref data[second + n + 3]);
				}

				//moving counters to next bit-reversed indexes
				second += 4;
				if (second >= n)
					break;
				int finder = top;
				while (first >= finder)
				{
					first -= finder;
					finder /= 2;
				}
				first += finder;
			}
		}

		public static void Normalize(double[] data)
		{
			int n = data.Length / 2; //div 2 because of Re+Img
			for (int i = 0; i < data.Length; i++)
			{
				data[i] *= Math.Pow(n, -1 / 2);
			}
		}

		public static bool IsPowOfTwo(int n)
		{
			if ((n & (n - 1)) != 0)
				return false;
			return true;
		}

		public static T Swap<T>(this T first, ref T second)
		{
			T tmp = second;
			second = first;
			return tmp;
		}
	}
}
