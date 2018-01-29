using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject.GUI.MenuItem.Edit
{
    class CutMenuItem : AbsMenuItem<String>
    {

        public CutMenuItem(AudioPanel entry) : base("Cut")
        {
            this.entry = entry;
        }

        public override void Item_Click(object sender, EventArgs e)
        {
            if (Service.DLLWrapper.Safe)
            {
                Service.EditTimeDomain edit = new Service.EditTimeDomain(entry);
                edit.Copy();
                edit.Delete();
            }
            else
            {
                Console.WriteLine("Unsafe Cut attempted and ignored.");
            }
        }

        private AudioPanel entry;
    }
}
