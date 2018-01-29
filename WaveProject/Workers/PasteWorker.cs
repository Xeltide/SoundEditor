using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject
{
    class PasteWorker : BackgroundWorker
    {

        public PasteWorker(AudioPanel entry)
        {
            this.panel = entry;
            this.DoWork += new DoWorkEventHandler(Paste);
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CallbackMain);
            panel.ParentRef.Enable_Edit(false);

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
        }

        protected void Paste(object sender, EventArgs e)
        {
            if (Service.DLLWrapper.Safe)
            {
                Cursor cursor = panel.TimeChannel1.ChartArea.CursorX;
                if (panel.PanelData.RawChartData.Count > 0 && panel.ParentRef.ScrollData.IntBuffer.Count > 0)
                {
                    for (int i = 0; i < panel.PanelData.RIFFData.Channels; i++)
                    {
                        int pos = (int)cursor.Position * (panel.PanelData.RIFFData.BitsPerSample / 8);

                        List<int> copyBuffer;
                        if (i < panel.ParentRef.ScrollData.IntBuffer.Count)
                        {
                            copyBuffer = panel.ParentRef.ScrollData.IntBuffer[i];
                        } else
                        {
                            copyBuffer = panel.ParentRef.ScrollData.IntBuffer[0];
                        }

                        RIFFData copyRiff = panel.ParentRef.ScrollData.BufferRIFF;
                        RIFFData curChRiff = panel.PanelData.RIFFData;
                        // Downsample
                        if (copyRiff.SampleRate > panel.PanelData.RIFFData.SampleRate)
                        {

                            // Generate filter
                            double freqPBin = (double)copyRiff.SampleRate / copyBuffer.Count;
                            int pasteFMax = curChRiff.SampleRate / 2;
                            int bin = (int)(pasteFMax / freqPBin);
                            bool bitOffset = (copyRiff.BitsPerSample == 8) != (panel.PanelData.RIFFData.BitsPerSample == 8);
                            FilterAndConvolve(copyBuffer, bin, bitOffset);

                            // Trim copy buffer
                            double sampleRatio = (double)curChRiff.SampleRate / copyRiff.SampleRate;
                            Func<int, int> mapFunction = Structure.BitMapper.GetMapper(copyRiff.BitsPerSample, panel.PanelData.RIFFData.BitsPerSample);
                            List<int> trimCopy = Service.SignalProcessing.Downsample(convolutionResult, sampleRatio, mapFunction);

                            if (i < 2)
                            {
                                List<int> input = panel.PanelData.RawChartData[i];
                                int index = (int)cursor.Position;
                                input.InsertRange((int)cursor.Position, trimCopy);
                            }

                            // Move new list into channel data
                            panel.PanelData.RawChannelData[i].InsertRange(pos, panel.PanelData.IntToByteList(trimCopy, copyRiff));
                            Console.WriteLine("Downsample complete");
                        }
                        // Upsample
                        else if (copyRiff.SampleRate < panel.PanelData.RIFFData.SampleRate)
                        {
                            // Load stuffed copy samples
                            double sampleRatio = (double)panel.PanelData.RIFFData.SampleRate / panel.ParentRef.ScrollData.BufferRIFF.SampleRate;
                            Func<int, int> mapFunction = Structure.BitMapper.GetMapper(copyRiff.BitsPerSample, panel.PanelData.RIFFData.BitsPerSample);
                            List<int> padCopy = Service.SignalProcessing.Upsample(copyBuffer, sampleRatio, mapFunction);

                            // Filter frequencies above receiver buffer fmax from copy
                            double freqPBin = (double)panel.PanelData.RIFFData.SampleRate / copyBuffer.Count;
                            int pasteFMax = panel.ParentRef.ScrollData.BufferRIFF.SampleRate / 2;
                            int bin = (int)(pasteFMax / freqPBin);
                            bool bitOffset = (copyRiff.BitsPerSample != panel.PanelData.RIFFData.BitsPerSample);
                            FilterAndConvolve(padCopy, bin, bitOffset);

                            // Move new list into channel data
                            if (i < 2)
                            {
                                List<int> input = panel.PanelData.RawChartData[i];
                                int index = (int)cursor.Position;
                                input.InsertRange((int)cursor.Position, convolutionResult);
                            }
                            List<byte> temp = panel.PanelData.IntToByteList(convolutionResult, panel.PanelData.RIFFData);
                            panel.PanelData.RawChannelData[i].InsertRange(pos, temp);
                            Console.WriteLine("Upsample complete");
                        }
                        // Copy is same sample rate
                        else
                        {
                            // Modify bits per sample values
                            if (copyRiff.BitsPerSample != panel.PanelData.RIFFData.BitsPerSample) {
                                Func<int, int> mapFunction = Structure.BitMapper.GetMapper(copyRiff.BitsPerSample, panel.PanelData.RIFFData.BitsPerSample);
                                List<int> adjustedBitRate = new List<int>();
                                foreach (int sample in copyBuffer)
                                {
                                    adjustedBitRate.Add(mapFunction(sample));
                                }

                                if (i < 2)
                                {
                                    List<int> input = panel.PanelData.RawChartData[i];
                                    int index = (int)cursor.Position;
                                    input.InsertRange((int)cursor.Position, adjustedBitRate);
                                }
                                panel.PanelData.RawChannelData[i].InsertRange(pos, panel.PanelData.IntToByteList(adjustedBitRate, panel.PanelData.RIFFData));
                            } else
                            {
                                if (i < 2)
                                {
                                    List<int> input = panel.PanelData.RawChartData[i];
                                    int index = (int)cursor.Position;
                                    input.InsertRange((int)cursor.Position, copyBuffer);
                                }
                                panel.PanelData.RawChannelData[i].InsertRange(pos, panel.PanelData.IntToByteList(copyBuffer, copyRiff));
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Unsafe Paste() attempted; ignored");
            }
        }

        protected void CallbackMain(object sender, EventArgs e)
        {
            // Adds last point, since it has changed and needs to expand the scroll range
            AudioChannel load = panel.TimeChannel1;
            if (load.Series.Points[load.Series.Points.Count - 1].XValue != panel.PanelData.RawChartData[0].Count - 1)
            {
                load.Series.Points.AddXY(panel.PanelData.RawChartData[0].Count - 1, panel.PanelData.RawChartData[0][panel.PanelData.RawChartData[0].Count - 1]);
            }
            if (panel.PanelData.RIFFData.Channels > 1)
            {
                load = panel.TimeChannel2;
                if (load.Series.Points[load.Series.Points.Count - 1].XValue != panel.PanelData.RawChartData[1].Count - 1)
                {
                    load.Series.Points.AddXY(panel.PanelData.RawChartData[1].Count - 1, panel.PanelData.RawChartData[1][panel.PanelData.RawChartData[1].Count - 1]);
                }
            }

            // Forced zoom to add scrollbar to new chunk
            double viewMin = panel.TimeChannel1.ChartArea.AxisX.ScaleView.ViewMinimum;
            double viewMax = panel.TimeChannel1.ChartArea.AxisX.ScaleView.ViewMaximum;
            panel.TimeChannel1.ChartArea.AxisX.ScaleView.Zoom(viewMin, viewMax - viewMin);

            // Refresh the viewable area
            panel.TimeChannel1.RefreshViewablePoints();

            if (panel.TimeChannel2 != null)
            {
                panel.TimeChannel2.ChartArea.AxisX.ScaleView.Zoom(viewMin, viewMax - viewMin);
                panel.TimeChannel2.RefreshViewablePoints();
            }

            // Repackage data for playback
            WavPackager packager = new WavPackager(null);
            panel.PanelData.RIFFData = packager.GenerateRIFFData(panel);
            panel.ParentRef.Enable_Edit(true);
            Console.WriteLine("Paste RIFF generated");
        }

        private void FilterAndConvolve(List<int> copyBuffer, int binNumber, bool bitOffset)
        {
            List<ComplexNumber> filter = Service.SignalProcessing.GenerateFilter(copyBuffer.Count, binNumber);

            // Convert filter to time domain
            int count = filter.Count;

            threads[0] = new Thread(() => ThreadedIDFT(filter, 0, 0, count / 4));
            threads[1] = new Thread(() => ThreadedIDFT(filter, 1, count / 4, count / 2));
            threads[2] = new Thread(() => ThreadedIDFT(filter, 2, count / 2, count * 3 / 4));
            threads[3] = new Thread(() => ThreadedIDFT(filter, 3, count * 3 / 4, count));

            threads[0].Start();
            threads[1].Start();
            threads[2].Start();
            threads[3].Start();

            idftResult = new List<int>();
            for (int t = 0; t < threads.Length; t++)
            {
                threads[t].Join();
                idftResult.AddRange(idftResults[t]);
            }

            // Apply filter
            count = copyBuffer.Count;

            threads[0] = new Thread(() => ThreadedConvolution(copyBuffer, 0, 0, count / 4, bitOffset));
            threads[1] = new Thread(() => ThreadedConvolution(copyBuffer, 1, count / 4, count / 2, bitOffset));
            threads[2] = new Thread(() => ThreadedConvolution(copyBuffer, 2, count / 2, count * 3 / 4, bitOffset));
            threads[3] = new Thread(() => ThreadedConvolution(copyBuffer, 3, count * 3 / 4, count, bitOffset));

            threads[0].Start();
            threads[1].Start();
            threads[2].Start();
            threads[3].Start();

            convolutionResult = new List<int>();
            for (int t = 0; t < threads.Length; t++)
            {
                threads[t].Join();
                convolutionResult.AddRange(convolutionResults[t]);
            }
        }

        private void ThreadedIDFT(List<ComplexNumber> filter, int index, int sampleBegin, int sampleEnd)
        {
            idftResults[index] = FourierMath.IDFTIntegers(filter, sampleBegin, sampleEnd);
        }

        private void ThreadedConvolution(List<int> buffer, int index, int start, int end, bool bitChange)
        {
            if (bitChange)
            {
                convolutionResults[index] = Service.SignalProcessing.ConvolveBitOffset(buffer, idftResult, start, end);
            }
            else
            {
                convolutionResults[index] = Service.SignalProcessing.Convolve(buffer, idftResult, start, end);
            }
        }

        private AudioPanel panel;

        private List<int> idftResult;
        private List<List<int>> idftResults;

        private List<int> convolutionResult;
        private List<List<int>> convolutionResults;

        Thread[] threads;
    }
}
