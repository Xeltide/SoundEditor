using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Encoding {
    class Bit16MenuItem : AbsMenuItem<short> {

        public Bit16MenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("16 bit") {
            this.entry = entry;
            this.manager = manager;
            this.value = 16;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.SetBits(1);
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
