using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Encoding {
    class khz44MenuItem : AbsMenuItem<int> {

        public khz44MenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("44,100 Hz") {
            this.entry = entry;
            this.manager = manager;
            this.value = 44100;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.SetKHZ(3);
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
