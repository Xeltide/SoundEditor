using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Encoding {
    class Bit8MenuItem : AbsMenuItem<short> {

        public Bit8MenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("8 bit") {
            this.entry = entry;
            this.manager = manager;
            this.Checked = true;
            this.value = 8;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.SetBits(0);
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
