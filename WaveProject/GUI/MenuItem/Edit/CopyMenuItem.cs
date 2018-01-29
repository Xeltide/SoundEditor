using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject.GUI.MenuItem.Edit
{
    class CopyMenuItem : AbsMenuItem<String>
    {

        public CopyMenuItem(AudioPanel entry) : base("Copy")
        {
            this.entry = entry;
        }

        public override void Item_Click(object sender, EventArgs e)
        {
            if (Service.DLLWrapper.Safe)
            {
                Service.EditTimeDomain edit = new Service.EditTimeDomain(entry);
                edit.Copy();
            }
            else
            {
                Console.WriteLine("Unsafe Copy attempted and ignored.");
            }

        }

        private AudioPanel entry;
    }
}
