using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem {
    class MonoMenuItem : AbsMenuItem<String> {

        public MonoMenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("Mono") {
            this.entry = entry;
            this.manager = manager;
            this.Checked = true;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.Mono_Click();
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
