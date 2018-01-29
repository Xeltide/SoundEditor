using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Fourier {
    class RunFourierMenuItem : AbsMenuItem<String> {

        public RunFourierMenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("Run On Selection") {
            this.entry = entry;
            this.manager = manager;
            this.Running = false;
        }

        public override void Item_Click(object sender, EventArgs e) {
            if (!Running) {
                Workers.FourierWorker worker1 = new Workers.FourierWorker(entry, this, manager.WindowMode, 0);
                worker1.RunWorkerAsync();
                if (entry.PanelData.RawChartData.Count > 1)
                {
                    Workers.FourierWorker worker2 = new Workers.FourierWorker(entry, this, manager.WindowMode, 1);
                    worker2.RunWorkerAsync();
                }
            }
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
        public bool Running { get; set; }
    }
}
