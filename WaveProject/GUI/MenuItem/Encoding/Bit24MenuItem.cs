using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Encoding {
    class Bit24MenuItem : AbsMenuItem<short> {

        public Bit24MenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("24 bit") {
            this.entry = entry;
            this.manager = manager;
            this.value = 24;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.SetBits(2);
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
