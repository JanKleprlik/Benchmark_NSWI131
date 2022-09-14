using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAud = NAudio.Dsp;

namespace FFTBenchmark.Benchmarks
{
    public class NAudioBenchmark : BenchmarkBase
    {
        protected override bool usesComplex => true;

        private NAud.Complex[] dataComplexNAud;

        public override void Prepare()
        {
            base.Prepare();
            dataComplexNAud = dataComplex.Select(x => new NAud.Complex { X = (float)x.Real, Y = (float)x.Imaginary }).ToArray();
        }

        [Benchmark]
        public void NAudio() =>
            /// <summary>
            /// - obtained from: https://github.com/naudio/NAudio/blob/master/NAudio.Core/Dsp/FastFourierTransform.cs
            /// This computes an in-place complex-to-complex FFT 
            /// x and y are the real and imaginary arrays of 2^m points.
            /// </summary>            
            NAud.FastFourierTransform.FFT(true, (int)Math.Log(Size, 2), dataComplexNAud);
    }
}
