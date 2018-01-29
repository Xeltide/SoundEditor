using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;
using System.Windows.Forms;

namespace WaveProject.GUI.MenuItem.File {
    class OpenMenuItem : AbsMenuItem<String> {

        public OpenMenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("Open") {
            this.entry = entry;
            this.manager = manager;
        }

        public override void Item_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = @"C:\";
            ofd.Title = "Browse WAV Files";

            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            ofd.DefaultExt = "wav";
            ofd.Filter = "WAV files (*.wav) | *.wav";
            ofd.RestoreDirectory = true;

            ofd.ReadOnlyChecked = true;
            ofd.ShowReadOnly = true;

            if (ofd.ShowDialog() == DialogResult.OK) {
                entry.PanelData.SetFilePath(ofd.FileName);
                entry.PanelData.SplitWavData();
                entry.ClearAllCharts();

                entry.PanelData.RawChartData.Add(entry.PanelData.GetReadableChannel(0));

                Workers.ChartRenderWorker worker = new Workers.ChartRenderWorker(entry, 0, true);
                worker.RunWorkerAsync();

                if (entry.PanelData.RawChannelData.Count > 1) {
                    manager.Stereo_Click();
                    manager.Mono.Enabled = false;

                    entry.PanelData.RawChartData.Add(entry.PanelData.GetReadableChannel(1));

                    Workers.ChartRenderWorker worker2 = new Workers.ChartRenderWorker(entry, 1, true);
                    worker2.RunWorkerAsync();
                } else {
                    manager.Mono_Click();
                    manager.Stereo.Enabled = false;
                }
            }

            manager.Open_Click();
            if (entry.PanelData.RawChartData.Count != 0)
            {
                entry.PlayBar.Enable_Record(false);
            }
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
