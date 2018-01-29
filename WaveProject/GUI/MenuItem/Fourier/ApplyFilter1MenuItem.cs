using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Fourier
{
    class ApplyFilter1MenuItem : AbsMenuItem<String>
    {

        public ApplyFilter1MenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("Apply Filter Mono / L")
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

            Workers.FilterWorker worker = new Workers.FilterWorker(entry, this, filter, 0);
            worker.RunWorkerAsync();

            Console.WriteLine("Completed filtering Mono / L channel");
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
