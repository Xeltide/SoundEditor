using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.File
{
    class CloseMenuItem : AbsMenuItem<String>
    {

        public CloseMenuItem(AudioPanel entry, AudioPanelMenuManager manager) : base("Close")
        {
            this.entry = entry;
            this.manager = manager;
        }

        public override void Item_Click(object sender, EventArgs e)
        {
            entry.ParentRef.Remove_Channel(entry);
        }

        private AudioPanel entry;
        private AudioPanelMenuManager manager;
    }
}
