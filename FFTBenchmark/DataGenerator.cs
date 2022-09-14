using MathNet.Numerics;

namespace FFTBenchmark
{
    
    public static class DataGenerator
    {
        static double frequency = 32d;
        static double amplitude = 128d;
        static double samplingRate = 512d;
        static int periode = (int)(samplingRate / frequency);


        public static double[] Sine(int length, double samplingRate, double frequency, double amplitude)
        {
            var data = Generate.Sinusoidal(length/2, samplingRate, frequency, amplitude);
            
            var result = new double[length];
            for(int i = 0; i < length/2; i++)
            {
                // real part
                result[i * 2] = data[i];
                // imaginary part
                result[i * 2 + 1] = 0d;
            }

            return result;
        }
        
        
        public static double[] CombinedSine(int length, double samplingRate, double frequency, double amplitude)
        {
            var sinusoidal1 = Generate.Sinusoidal(length/2, samplingRate, frequency * 0.7, amplitude*0,7);
            var sinusoidal2 = Generate.Sinusoidal(length/2, samplingRate, frequency, amplitude);
            var sinusoidal3 = Generate.Sinusoidal(length/2, samplingRate, frequency * 1.3, amplitude*1,3);

            var result = new double[length];
            for (int i = 0; i < length; i++)
            {
                // real part
                result[i*2] = sinusoidal1[i] + sinusoidal2[i] + sinusoidal3[i];
                // imaginary part
                result[i * 2 + 1] = 0d;
            }

            return result;
        }
    }
}
