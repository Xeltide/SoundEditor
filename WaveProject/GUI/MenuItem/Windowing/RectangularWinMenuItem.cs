using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Windowing {
    class RectangularWinMenuItem : AbsMenuItem<String> {

        public RectangularWinMenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("None") {
            this.entry = entry;
            this.manager = manager;

            this.Checked = true;
            manager.WindowMode = Structure.TypeKey.Windowing.Rectangular;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.WindowMode = Structure.TypeKey.Windowing.Rectangular;

            this.Checked = true;
            manager.Tri.Checked = false;
            manager.Welch.Checked = false;
            manager.Sine.Checked = false;
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
