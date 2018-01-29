using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject.Service
{
    class EditTimeDomain
    {

        public EditTimeDomain(AudioPanel panel)
        {
            this.panel = panel;
        }

        public void Copy()
        {
            if (Service.DLLWrapper.Safe)
            {
                Cursor time1 = panel.TimeChannel1.ChartArea.CursorX;

                if (panel.PanelData.RawChartData.Count > 0)
                {
                    if (time1.SelectionStart != time1.SelectionEnd)
                    {
                        panel.PanelData.IntBuffer = new List<List<int>>();
                        panel.ParentRef.ScrollData.IntBuffer = new List<List<int>>();
                        panel.ParentRef.ScrollData.BufferRIFF = panel.PanelData.RIFFData;
                        int smallest = (int)Math.Min(time1.SelectionStart, time1.SelectionEnd);
                        int biggest = (int)Math.Max(time1.SelectionStart, time1.SelectionEnd);
                        for (int i = 0; i < panel.PanelData.RIFFData.Channels; i++)
                        {
                            List<int> temp = panel.PanelData.GetReadableChannel(i).GetRange(smallest, biggest - smallest);
                            panel.ParentRef.ScrollData.IntBuffer.Add(temp);
                            panel.PanelData.IntBuffer.Add(temp);
                        }
                    }
                }
            } else
            {
                Console.WriteLine("Unsafe Copy() attempted; ignored");
            }
        }

        public void Paste()
        {
            PasteWorker worker = new PasteWorker(panel);
            worker.RunWorkerAsync();
        }

        // Currently deletes same segment from multiple channels
        public void Delete()
        {
            if (Service.DLLWrapper.Safe)
            {
                // set cursor based on current panel
                Cursor cursor = panel.TimeChannel1.ChartArea.CursorX;
                int smaller = (int)Math.Min(cursor.SelectionStart, cursor.SelectionEnd);
                int bigger = (int)Math.Max(cursor.SelectionStart, cursor.SelectionEnd);

                WavPackager packager = new WavPackager(null);
                RIFFData riff;

                for (int i = 0; i < panel.PanelData.RawChartData.Count; i++)
                {
                    switch (panel.PanelData.RIFFData.BitsPerSample)
                    {
                        case 8:
                            panel.PanelData.RawChartData[i].RemoveRange(smaller, bigger - smaller);
                            panel.PanelData.RawChannelData[i].RemoveRange(smaller, bigger - smaller);
                            break;
                        case 16:
                            panel.PanelData.RawChartData[i].RemoveRange(smaller, bigger - smaller);
                            panel.PanelData.RawChannelData[i].RemoveRange(smaller * 2, (bigger - smaller) * 2);
                            break;
                        case 24:
                            panel.PanelData.RawChartData[i].RemoveRange(smaller, bigger - smaller);
                            panel.PanelData.RawChannelData[i].RemoveRange(smaller * 3, (bigger - smaller) * 3);
                            break;
                        case 32:
                            panel.PanelData.RawChartData[i].RemoveRange(smaller, bigger - smaller);
                            panel.PanelData.RawChannelData[i].RemoveRange(smaller * 4, (bigger - smaller) * 4);
                            break;
                        default:
                            Console.WriteLine("DeleteWorker - Unsupported bits per sample");
                            break;
                    }
                }

                riff = packager.GenerateRIFFData(panel);
                panel.PanelData.RIFFData = riff;

                panel.TimeChannel1.RefreshViewablePoints();
                if (panel.TimeChannel2 != null)
                {
                    panel.TimeChannel2.RefreshViewablePoints();
                }
            } else
            {
                Console.WriteLine("Unsafe Delete() attempted; ignored");
            }
        }

        private AudioPanel panel;
    }
}
