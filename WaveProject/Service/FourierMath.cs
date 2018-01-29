using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using WaveProject.Structure;

namespace WaveProject {
    class FourierMath {

        public static List<ComplexNumber> DFTIntegers(List<int> input, TypeKey.Windowing type, RIFFData riff, int binStart, int binEnd) {
            List<ComplexNumber> output = new List<ComplexNumber>();
            for (int f = binStart; f < binEnd; f++) {
                double reSum = 0;
                double imSum = 0;
                for (int t = 0; t < input.Count; t++) {
                    double weight = GetWindowing(t, input.Count, type);
                    double angle = (2 * Math.PI * t * f) / input.Count;
                    reSum += weight * input[t] * Math.Cos(angle);
                    imSum -= weight * input[t] * Math.Sin(angle);
                }
                reSum /= input.Count;
                imSum /= input.Count;
                ComplexNumber n = new ComplexNumber();
                n.Real = reSum;
                n.Imaginary = imSum;
                n.SetLength();
                output.Add(n);
            }
            return output;
        }
        
        public static List<int> IDFTIntegers(List<ComplexNumber> input, int sampleBegin, int sampleEnd) {
            List<int> output = new List<int>();
            for (int t = sampleBegin; t < sampleEnd; t++) {
                double sampSum = 0;
                for (int f = 0; f < input.Count; f++) {
                    double angle = 2 * Math.PI * t * f / input.Count;
                    sampSum += (input[f].Real * Math.Cos(angle)) - (input[f].Imaginary * Math.Sin(angle));
                }
                output.Add((int)sampSum);
            }
            return output;
        }

        private static double GetWindowing(int n, int N, TypeKey.Windowing type) {
            switch (type) {
                case TypeKey.Windowing.Triangular:
                    int L = N - 1;
                    return 1 - Math.Abs((n - ((N - 1) / 2)) / (L / 2));
                case TypeKey.Windowing.Welch:
                    return 1 - Math.Pow((n - ((N - 1) / 2)) / ((N - 1) / 2), 2);
                case TypeKey.Windowing.Sine:
                    return Math.Sin((Math.PI * n) / (N - 1));
                default:
                    return 1;
            }
        }
    }
}
