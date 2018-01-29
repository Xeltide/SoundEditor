using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Encoding {
    class Bit32MenuItem : AbsMenuItem<short> {

        public Bit32MenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("32 bit") {
            this.entry = entry;
            this.manager = manager;
            this.value = 32;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.SetBits(3);
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
