using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace WaveProject.Workers {
    class FourierWorker : BackgroundWorker {

        public FourierWorker(AudioPanel entry, GUI.MenuItem.Fourier.RunFourierMenuItem button, Structure.TypeKey.Windowing type, int channelIndex) {
            this.entry = entry;
            this.button = button;
            this.DoWork += new DoWorkEventHandler(RunDFTOnSelection);
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CallbackAsync);
            this.chIndex = channelIndex;
            this.type = type;
            this.threads = new Thread[4];
            dftResults = new List<List<ComplexNumber>>();
            dftResults.Add(new List<ComplexNumber>());
            dftResults.Add(new List<ComplexNumber>());
            dftResults.Add(new List<ComplexNumber>());
            dftResults.Add(new List<ComplexNumber>());

            if (channelIndex == 0) {
                ch = entry.TimeChannel1;
            } else if (channelIndex == 1) {
                ch = entry.TimeChannel2;
            }
            button.Enabled = false;
        }

        protected void RunDFTOnSelection(object sender, EventArgs e) {
            if (ch.ChartArea.CursorX.SelectionEnd != ch.ChartArea.CursorX.SelectionStart) {
                button.Running = true;
                int small = (int)Math.Min(ch.ChartArea.CursorX.SelectionEnd, ch.ChartArea.CursorX.SelectionStart);
                int big = (int)Math.Max(ch.ChartArea.CursorX.SelectionEnd, ch.ChartArea.CursorX.SelectionStart);
                Console.WriteLine("Running DFT.");
                List<int> selection = entry.PanelData.RawChartData[chIndex].GetRange(small, big - small);
                int count = big - small;

                threads[0] = new Thread(() => ThreadedDFT(selection, entry.PanelData.RIFFData, 0, 0, count / 4));
                threads[1] = new Thread(() => ThreadedDFT(selection, entry.PanelData.RIFFData, 1, count / 4, count / 2));
                threads[2] = new Thread(() => ThreadedDFT(selection, entry.PanelData.RIFFData, 2, count / 2, count * 3 / 4));
                threads[3] = new Thread(() => ThreadedDFT(selection, entry.PanelData.RIFFData, 3, count * 3 / 4, count));

                threads[0].Start();
                threads[1].Start();
                threads[2].Start();
                threads[3].Start();
                
                threads[0].Join();
                threads[1].Join();
                threads[2].Join();
                threads[3].Join();
                Console.WriteLine("DFT completed.");
            }
        }

        private void ThreadedDFT(List<int> selection, RIFFData riff, int index, int binStart, int binEnd)
        {
            dftResults[index] = FourierMath.DFTIntegers(selection, type, riff, binStart, binEnd);
        }

        protected void CallbackAsync(object sender, EventArgs e) {
            FrequencyChannel fCh;
            if (chIndex == 0) {
                fCh = entry.FrequencyChannel1;
            } else if (chIndex == 1) {
                fCh = entry.FrequencyChannel2;
            } else {
                fCh = null;
            }

            if (fCh != null) {

                fCh.Series.Points.Clear();
                dftResult = new List<ComplexNumber>();
                for (int i = 0; i < dftResults.Count; i++)
                {
                    dftResult.AddRange(dftResults[i]);
                }
                
                for (int i = 0; i < dftResult.Count; i++) {
                    double f = (double)(i * entry.PanelData.RIFFData.SampleRate) / (double)dftResult.Count;
                    fCh.Series.Points.AddXY(f, dftResult[i].Length);
                }
            }
            button.Running = false;
            button.Enabled = true;
        }

        private AudioPanel entry;
        private int chIndex;
        private AudioChannel ch;
        private List<ComplexNumber> dftResult;
        private GUI.MenuItem.Fourier.RunFourierMenuItem button;
        private Structure.TypeKey.Windowing type;
        private Thread[] threads;
        private List<List<ComplexNumber>> dftResults;
    }
}
