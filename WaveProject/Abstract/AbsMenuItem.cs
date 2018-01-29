using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WaveProject.Abstract {
    abstract class AbsMenuItem<T> : ToolStripMenuItem {

        public AbsMenuItem(String name) {
            this.Text = name;
            this.ForeColor = Color.White;
            this.Click += new EventHandler(Item_Click);
        }

        public void SetText(String text)
        {
            this.Text = text;
        }

        abstract public void Item_Click(object sender, EventArgs e);

        protected T value;
        public T Value {
            get {
                return value;
            }
        }
    }
}
