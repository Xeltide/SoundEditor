using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Encoding {
    class khz8MenuItem : AbsMenuItem<int> {

        public khz8MenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("8,000 Hz") {
            this.entry = entry;
            this.manager = manager;
            this.Checked = true;
            this.value = 8000;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.SetKHZ(0);
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
