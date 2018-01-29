using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveProject.Service
{
    class SignalProcessing
    {
        public static List<ComplexNumber> GenerateFilter(int binCount, int binNumber)
        {
            List<ComplexNumber> output = new List<ComplexNumber>();

            for (int b = 0; b <= binNumber; b++)
            {
                ComplexNumber n = new ComplexNumber();
                n.Real = 1;
                n.Imaginary = 0;
                output.Add(n);
            }

            for (int b = binNumber; b < binCount - binNumber; b++)
            {
                ComplexNumber n = new ComplexNumber();
                n.Real = 0;
                n.Imaginary = 0;
                output.Add(n);
            }

            for (int b = binCount - binNumber; b < binCount; b++)
            {
                ComplexNumber n = new ComplexNumber();
                n.Real = 1;
                n.Imaginary = 0;
                output.Add(n);
            }

            return output;
        }

        /* Convolution specifically for:
         * - 8 to X bit downsample
         * - 8 to X bit upsample
         */
        public static List<int> ConvolveBitOffset(List<int> inputSamples, List<int> filterSamples, int start, int end)
        {
            List<int> output = new List<int>();

            for (int sample = start; sample < end; sample++)
            {
                int sum = 0;
                for (int filter = 0; filter < filterSamples.Count; filter++)
                {
                    int samp;
                    if (sample + filter < inputSamples.Count)
                    {
                        samp = inputSamples[sample + filter] - 127;
                    } else {
                        samp = 0;
                    }
                    sum += samp * filterSamples[filter];
                }
                output.Add(sum / filterSamples.Count);
                output[output.Count - 1] += 127;
            }

            return output;
        }

        public static List<int> Convolve(List<int> inputSamples, List<int> filterSamples, int start, int end)
        {
            List<int> output = new List<int>();

            for (int sample = start; sample < end; sample++)
            {
                int sum = 0;
                for (int filter = 0; filter < filterSamples.Count; filter++)
                {
                    sum += (sample + filter >= inputSamples.Count ? 0 : inputSamples[sample + filter]) * filterSamples[filter];
                }
                output.Add(sum / filterSamples.Count);
            }

            return output;
        }

        public static List<int> Upsample(List<int> src, double sampleRatio, Func<int, int> bitmapperFunction)
        {
            List<int> output = new List<int>();

            double count = 0;
            for (int i = 0; i < src.Count; i++)
            {
                count += sampleRatio;
                while (count >= 1)
                {
                    int temp = bitmapperFunction(src[i]);
                    output.Add(temp);
                    count--;
                }
            }

            return output;
        }

        public static List<int> Downsample(List<int> src, double sampleRatio, Func<int, int> bitmapperFunction)
        {
            List<int> output = new List<int>();

            double count = 0;
            for (int i = 0; i < src.Count; i++)
            {
                count += sampleRatio;

                if (count >= 1)
                {
                    count--;
                    int temp = bitmapperFunction(src[i]);
                    output.Add(temp);
                }
            }

            return output;
        }
    }
}
