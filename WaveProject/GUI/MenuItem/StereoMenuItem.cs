using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;
using System.Drawing;

namespace WaveProject.GUI.MenuItem {
    class StereoMenuItem : AbsMenuItem<String> {

        public StereoMenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("Stereo") {
            this.entry = entry;
            this.manager = manager;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.Stereo_Click();
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
