using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace WaveProject.Workers
{
    class FilterWorker : BackgroundWorker
    {

        public FilterWorker(AudioPanel entry, Abstract.AbsMenuItem<String> button, List<ComplexNumber> filter, int channelIndex)
        {
            this.entry = entry;
            this.button = button;
            this.filter = filter;
            this.DoWork += new DoWorkEventHandler(RunFilterOnSelection);
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CallbackAsync);
            this.chIndex = channelIndex;
            this.threads = new Thread[4];

            idftResults = new List<List<int>>();
            idftResults.Add(new List<int>());
            idftResults.Add(new List<int>());
            idftResults.Add(new List<int>());
            idftResults.Add(new List<int>());

            convolutionResults = new List<List<int>>();
            convolutionResults.Add(new List<int>());
            convolutionResults.Add(new List<int>());
            convolutionResults.Add(new List<int>());
            convolutionResults.Add(new List<int>());
            button.Enabled = false;
        }

        protected void RunFilterOnSelection(object sender, EventArgs e)
        {
            // Filter to time domain
            Console.WriteLine("Running IDFT");
            int count = filter.Count;

            threads[0] = new Thread(() => ThreadedIDFT(0, 0, count / 4));
            threads[1] = new Thread(() => ThreadedIDFT(1, count / 4, count / 2));
            threads[2] = new Thread(() => ThreadedIDFT(2, count / 2, count * 3 / 4));
            threads[3] = new Thread(() => ThreadedIDFT(3, count * 3 / 4, count));

            threads[0].Start();
            threads[1].Start();
            threads[2].Start();
            threads[3].Start();

            idftResult = new List<int>();
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
                idftResult.AddRange(idftResults[i]);
            }

            count = entry.PanelData.RawChartData[chIndex].Count;
            Console.WriteLine("idftResult: " + count);

            threads[0] = new Thread(() => ThreadedConvolution(0, 0, count / 4));
            threads[1] = new Thread(() => ThreadedConvolution(1, count / 4, count / 2));
            threads[2] = new Thread(() => ThreadedConvolution(2, count / 2, count * 3 / 4));
            threads[3] = new Thread(() => ThreadedConvolution(3, count * 3 / 4, count));

            threads[0].Start();
            threads[1].Start();
            threads[2].Start();
            threads[3].Start();

            convolutionResult = new List<int>();
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
                convolutionResult.AddRange(convolutionResults[i]);
            }

            // Apply filter to time domain
            entry.PanelData.RawChartData[chIndex] = convolutionResult;

            // Repackage byte data for playback
            entry.PanelData.RawChannelData[chIndex] = entry.PanelData.IntToByteList(entry.PanelData.RawChartData[chIndex], entry.PanelData.RIFFData);
            WavPackager packager = new WavPackager(null);
            entry.PanelData.RIFFData = packager.GenerateRIFFData(entry);

            Console.WriteLine("IDFT completed");
        }

        private void ThreadedIDFT(int index, int sampleBegin, int sampleEnd)
        {
            idftResults[index] = FourierMath.IDFTIntegers(filter, sampleBegin, sampleEnd);
        }

        private void ThreadedConvolution(int index, int start, int end)
        {
            convolutionResults[index] = Service.SignalProcessing.Convolve(entry.PanelData.RawChartData[chIndex], idftResult, start, end);
        }

        protected void CallbackAsync(object sender, EventArgs e)
        {
            if (chIndex == 0)
            {
                entry.TimeChannel1.RefreshViewablePoints();
            } else if (chIndex == 1)
            {
                entry.TimeChannel2.RefreshViewablePoints();
            }
            button.Enabled = true;
        }

        private AudioPanel entry;
        private int chIndex;

        private List<ComplexNumber> filter;
        private List<int> idftResult;
        private List<List<int>> idftResults;

        private List<int> convolutionResult;
        private List<List<int>> convolutionResults;

        private Thread[] threads;
        private Abstract.AbsMenuItem<String> button;
    }
}
