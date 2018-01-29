using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Windowing {
    class SineWinMenuItem : AbsMenuItem<String> {

        public SineWinMenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("Sine") {
            this.entry = entry;
            this.manager = manager;
        }

        public override void Item_Click(object sender, EventArgs e) {
            manager.WindowMode = Structure.TypeKey.Windowing.Sine;

            this.Checked = true;
            manager.Rect.Checked = false;
            manager.Tri.Checked = false;
            manager.Welch.Checked = false;
        }

        AudioPanel entry;
        AudioPanelMenuManager manager;
    }
}
