using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject.GUI.MenuItem.Edit
{
    class PasteMenuItem : AbsMenuItem<String>
    {

        public PasteMenuItem(AudioPanel entry) : base("Paste")
        {
            this.entry = entry;
        }

        public override void Item_Click(object sender, EventArgs e)
        {
            Service.EditTimeDomain edit = new Service.EditTimeDomain(entry);
            edit.Paste();
        }

        private AudioPanel entry;
    }
}
