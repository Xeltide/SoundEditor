using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Fourier
{
    class ApplyFilter2MenuItem : AbsMenuItem<String>
    {

        public ApplyFilter2MenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("Apply Filter R")
        {
            this.entry = entry;
            this.manager = manager;
        }

        public override void Item_Click(object sender, EventArgs e)
        {
            FrequencyChannel f = entry.FrequencyChannel1;

            // Generate filter
            int bin = (int)((f.ChartArea.CursorX.Position * f.Series.Points.Count / entry.PanelData.RIFFData.SampleRate) + 0.5);
            List<ComplexNumber> filter = Service.SignalProcessing.GenerateFilter(f.Series.Points.Count, bin);

            Workers.FilterWorker worker = new Workers.FilterWorker(entry, this, filter, 1);
            worker.RunWorkerAsync();

            Console.WriteLine("Completed filtering R channel");
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
