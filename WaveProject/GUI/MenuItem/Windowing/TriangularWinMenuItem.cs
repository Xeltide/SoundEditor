using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Windowing {
    class TriangularWinMenuItem : AbsMenuItem<String> {

        public TriangularWinMenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("Bartlett") {
            this.entry = entry;
            this.manager = manager;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.WindowMode = Structure.TypeKey.Windowing.Triangular;

            this.Checked = true;
            manager.Rect.Checked = false;
            manager.Welch.Checked = false;
            manager.Sine.Checked = false;
        }

        AudioPanel entry;
        AudioPanelMenuManager manager;
    }
}
