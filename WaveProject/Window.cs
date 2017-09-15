using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveProject {
    public partial class Window : Form {
        public Window() {
            InitializeComponent();
        }

        private void Window_Load(object sender, EventArgs e) {
            Size screen = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = new Point(screen.Width / 4, screen.Height / 4);
            this.ClientSize = new Size(screen.Width / 2, screen.Height / 2);

            acl = new AudioChannel("L Channel", 12, 36, this.ClientSize.Width - 24, (this.ClientSize.Height - 33) / 2);
            acr = new AudioChannel("R Channel", 12, ((30 + this.ClientSize.Height - 24) / 2) + 12, this.ClientSize.Width - 24, (this.ClientSize.Height - 33) / 2);

            this.Controls.Add(acl.Chart);
            this.Controls.Add(acr.Chart);
        }

        private AudioChannel acl;
        private AudioChannel acr;
    }
}
