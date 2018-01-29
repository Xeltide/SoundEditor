using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject.Workers {
    class ChartRenderWorker : BackgroundWorker {

        public ChartRenderWorker(AudioPanel entry, int channelIndex, bool initialLoad) {
            this.initialLoad = initialLoad;
            this.entry = entry;
            this.channelIndex = channelIndex;
            this.DoWork += new DoWorkEventHandler(NormalizeChartData);
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CallbackAsync);
        }

        protected void NormalizeChartData(object sender, EventArgs e)
        {
            if (entry.PanelData.RawChartData.Count > 0)
            {
                lock (entry.PanelData.RawChartData[channelIndex])
                {
                    AudioChannel load;
                    List<int> input = entry.PanelData.RawChartData[channelIndex];
                    outputH = new List<int>();
                    outputHI = new List<int>();
                    outputL = new List<int>();
                    outputLI = new List<int>();

                    if (initialLoad)
                    {
                        load = entry.TimeChannel1;

                        min = 0;
                        max = input.Count - 1;
                    }
                    else
                    {
                        if (channelIndex == 0)
                        {
                            load = entry.TimeChannel1;
                        }
                        else
                        {
                            load = entry.TimeChannel2;
                        }

                        min = load.ChartArea.AxisX.ScaleView.ViewMinimum;
                        max = load.ChartArea.AxisX.ScaleView.ViewMaximum;
                    }

                    double pxW = load.Chart.Width;
                    double valW = max - min;

                    interval = (int)(valW / pxW);
                    if (interval > 1)
                    {
                        cull = true;
                        int start = (int)min >= 1 ? (int)min - 1 : 0;
                        int stop = (int)max < input.Count ? (int)max + 1 : input.Count;
                        for (int i = start; i < stop; i += interval)
                        {
                            int high = input[i];
                            int hI = 0;
                            int low = input[i];
                            int lI = 0;
                            for (int j = 0; j < interval && j + i < input.Count; j++)
                            {
                                if (input[i + j] > high)
                                {
                                    high = input[i + j];
                                    hI = j;
                                }
                                if (input[i + j] < low)
                                {
                                    low = input[i + j];
                                    lI = j;
                                }
                            }
                            outputH.Add(high);
                            outputHI.Add(hI);
                            outputL.Add(low);
                            outputLI.Add(lI);
                        }
                    }
                    else
                    {
                        cull = false;
                    }
                }
            }
        }

        protected void CallbackAsync(object sender, EventArgs e) {
            AudioChannel load;
            if (channelIndex == 0) {
                load = entry.TimeChannel1;
            } else {
                load = entry.TimeChannel2;
            }

            if (entry.PanelData.RawChartData.Count > 0)
            {
                // Delete old points inside view
                if (load.Series.Points.Count > 0)
                {
                    int removeIndex = 0;
                    while (load.Series.Points.Count > removeIndex && load.Series.Points[removeIndex].XValue <= min)
                    {
                        removeIndex++;
                    }

                    while (load.Series.Points.Count > removeIndex && load.Series.Points[removeIndex].XValue <= max)
                    {
                        load.Series.Points.RemoveAt(removeIndex);
                    }
                }

                if (cull)
                {
                    for (int labelCount = 0; labelCount < load.ChartArea.AxisX.CustomLabels.Count; labelCount++)
                    {
                        load.ChartArea.AxisX.CustomLabels.RemoveAt(labelCount);
                    }
                    
                    double viewMin = load.ChartArea.AxisX.ScaleView.ViewMinimum;
                    double viewMax = load.ChartArea.AxisX.ScaleView.ViewMaximum;
                    double viewRange = viewMax - viewMin;
                    double viewScale = load.Series.Points.Count / viewRange;
                    double lowLabelVal = viewMin / (double)load.Panel.PanelData.RIFFData.SampleRate;
                    //CustomLabel lowLabel = new CustomLabel(viewMin, viewMin + 100, "" + ((int)(lowLabelVal * 100) / 100.0), 0, LabelMarkStyle.None);
                    //load.ChartArea.AxisX.CustomLabels.Add(lowLabel);

                    // Show only highest and lowest point per pixel
                    for (int i = 0; i < outputH.Count; i++)
                    {
                        load.Series.Points.AddXY(((i * interval) + outputHI[i] + load.ChartArea.AxisX.ScaleView.ViewMinimum), outputH[i]);
                        load.Series.Points.AddXY(((i * interval) + outputLI[i] + load.ChartArea.AxisX.ScaleView.ViewMinimum), outputL[i]);
                        //load.Series.Points.AddXY(((i * interval) + outputHI[i] + load.ChartArea.AxisX.ScaleView.ViewMinimum) / (double)load.Panel.PanelData.RIFFData.SampleRate, outputH[i]);
                        //load.Series.Points.AddXY(((i * interval) + outputLI[i] + load.ChartArea.AxisX.ScaleView.ViewMinimum) / (double)load.Panel.PanelData.RIFFData.SampleRate, outputL[i]);
                    }

                    // Make sure last point is loaded in
                    if (load.Series.Points[load.Series.Points.Count - 1].XValue != entry.PanelData.RawChartData[channelIndex].Count - 1)
                    {
                        load.Series.Points.AddXY(entry.PanelData.RawChartData[channelIndex].Count - 1, entry.PanelData.RawChartData[channelIndex][entry.PanelData.RawChartData[channelIndex].Count - 1]);
                        //load.Series.Points.AddXY((entry.PanelData.RawChartData[channelIndex].Count - 1) / (double)load.Panel.PanelData.RIFFData.SampleRate, entry.PanelData.RawChartData[channelIndex][entry.PanelData.RawChartData[channelIndex].Count - 1]);
                    }
                }
                else
                {
                    // All points shown in viewable area
                    for (int i = (int)min; i < (int)max; i++)
                    {
                        load.Series.Points.AddXY(i, entry.PanelData.RawChartData[channelIndex][i]);
                    }

                    int channelSize = entry.PanelData.RawChartData[channelIndex].Count;
                    if ((int)max < channelSize)
                    {
                        load.Series.Points.AddXY(channelSize - 1, entry.PanelData.RawChartData[channelIndex][channelSize - 1]);
                    }
                }

                load.Series.Sort(PointSortOrder.Ascending, "X");
            }
        }

        private bool initialLoad;
        private int channelIndex;
        private double min;
        private double max;
        private int interval;
        private bool cull;
        private List<int> outputH;
        private List<int> outputHI;
        private List<int> outputL;
        private List<int> outputLI;

        private AudioPanel entry;
    }
}
