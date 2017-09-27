using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace WaveProject {
    class FourierMath {

        public static List<ComplexNumber> DFTIntegers(List<int> input, RIFFData riff) {
            List<ComplexNumber> output = new List<ComplexNumber>();
            for (int hz = 0; hz < riff.SampleRate; hz++) {
                double reSum = 0;
                double imSum = 0;
                for (int time = 0; time < input.Count; time++) {
                    double angle = 2 * Math.PI * time * hz / riff.SampleRate;
                    reSum += input[time] * Math.Cos(angle);
                    imSum -= input[time] * Math.Sin(angle);
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

        // Temporary series to series data
        // Replace when data has moved to this class
        public static void TTFData(Series input, Series output, int sr) {
            for (int fBucket = 0; fBucket <= sr; fBucket++) {
                double sum = 0;
                for (int time = 0; time < input.Points.Count; time++) {
                    double sampleT = input.Points[time].YValues[0];
                    double yAtT = Math.Cos(2 * Math.PI * fBucket * time / input.Points.Count);
                    sum += sampleT * yAtT;
                }
                sum /= (sr / 2);
                output.Points.AddXY(fBucket, sum);
            }
        }
    }
}
