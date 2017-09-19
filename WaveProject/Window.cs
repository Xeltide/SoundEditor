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
            this.menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MenuColoursTable());
        }

        private void Window_Load(object sender, EventArgs e) {
            Size screen = Screen.PrimaryScreen.WorkingArea.Size;
            this.MinimumSize = new Size(640, 360);
            this.Location = new Point(screen.Width / 4, screen.Height / 4);
            this.ClientSize = new Size(screen.Width / 2, screen.Height / 2);
            
            bttnPanel = new ButtonPanel(new Point(0, 25), this.ClientSize);
            chPanel = new ChannelPanel(new Point(12, bttnPanel.Location.Y + bttnPanel.ClientSize.Height + 1), this.ClientSize);

            this.Controls.Add(bttnPanel);
            this.Controls.Add(chPanel);
        }

        private void Window_Resize(object sender, EventArgs e) {
            bttnPanel.ResizePanel(this.ClientSize);
            chPanel.ResizePanel(this.ClientSize);
        }

        private ButtonPanel bttnPanel;
        private ChannelPanel chPanel;
    }
}
