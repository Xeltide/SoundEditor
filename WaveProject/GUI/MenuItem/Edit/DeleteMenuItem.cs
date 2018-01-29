using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.Abstract;

namespace WaveProject.GUI.MenuItem.Edit
{
    class DeleteMenuItem : AbsMenuItem<String>
    {

        public DeleteMenuItem(AudioPanel entry) : base("Delete")
        {
            this.entry = entry;
        }

        public override void Item_Click(object sender, EventArgs e)
        {
            Service.EditTimeDomain edit = new Service.EditTimeDomain(entry);
            edit.Delete();
        }

        private AudioPanel entry;
    }
}
