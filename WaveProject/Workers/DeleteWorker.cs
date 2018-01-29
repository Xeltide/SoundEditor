using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject {
    class DeleteWorker : BackgroundWorker {

        public DeleteWorker(AudioPanel entry) {
            this.entry = entry;
            this.DoWork += new DoWorkEventHandler(ZeroYValues);
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CallbackAsync);
        }

        protected void ZeroYValues(object sender, EventArgs e) {
            // set cursor based on current panel
            cursor = entry.TimeChannel1.ChartArea.CursorX;

            smaller = (int)Math.Min(cursor.SelectionStart, cursor.SelectionEnd);
            bigger = (int)Math.Max(cursor.SelectionStart, cursor.SelectionEnd);
        }

        protected void CallbackAsync(object sender, EventArgs e) {
            WavPackager packager = new WavPackager(null);
            RIFFData riff;

            switch (entry.PanelData.RIFFData.BitsPerSample) {
                case 8:
                    entry.PanelData.RawChartData[0].RemoveRange(smaller, bigger - smaller);
                    entry.PanelData.RawChannelData[0].RemoveRange(smaller, bigger - smaller);
                    riff = packager.GenerateRIFFData(entry);
                    entry.PanelData.RIFFData = riff;
                    break;
                case 16:
                    entry.PanelData.RawChartData[0].RemoveRange(smaller, bigger - smaller);
                    entry.PanelData.RawChannelData[0].RemoveRange(smaller * 2, (bigger * 2) - (smaller * 2));
                    riff = packager.GenerateRIFFData(entry);
                    entry.PanelData.RIFFData = riff;
                    break;
                case 24:
                    entry.PanelData.RawChartData[0].RemoveRange(smaller, bigger - smaller);
                    entry.PanelData.RawChannelData[0].RemoveRange(smaller * 3, (bigger * 3) - (smaller * 3));
                    riff = packager.GenerateRIFFData(entry);
                    entry.PanelData.RIFFData = riff;
                    break;
                case 32:
                    entry.PanelData.RawChartData[0].RemoveRange(smaller, bigger - smaller);
                    entry.PanelData.RawChannelData[0].RemoveRange(smaller * 4, (bigger * 4) - (smaller * 4));
                    riff = packager.GenerateRIFFData(entry);
                    entry.PanelData.RIFFData = riff;
                    break;
                default:
                    Console.WriteLine("DeleteWorker - Unsupported bits per sample");
                    break;
            }

            entry.TimeChannel1.RefreshViewablePoints();
            if (entry.TimeChannel2 != null) {
                entry.TimeChannel2.RefreshViewablePoints();
            }
        }

        private Cursor cursor;
        private int smaller;
        private int bigger;

        private AudioPanel entry;
    }
}
