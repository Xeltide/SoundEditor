using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Encoding {
    class khz11MenuItem : AbsMenuItem<int> {

        public khz11MenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("11,025 Hz") {
            this.entry = entry;
            this.manager = manager;
            this.value = 11025;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.SetKHZ(1);
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
