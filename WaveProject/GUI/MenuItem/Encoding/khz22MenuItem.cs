using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Encoding {
    class khz22MenuItem : AbsMenuItem<int> {

        public khz22MenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("22,050 Hz") {
            this.entry = entry;
            this.manager = manager;
            this.value = 22050;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.SetKHZ(2);
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
