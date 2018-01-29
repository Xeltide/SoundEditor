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
            //this.menuStrip1.Renderer = new ToolStripProfessionalRenderer(new MenuColoursTable());
        }

        private void Window_Load(object sender, EventArgs e) {
            // temp static to initialize some panels
            window = this;

            Size screen = Screen.PrimaryScreen.WorkingArea.Size;
            this.MinimumSize = new Size(760, 460);
            this.Location = new Point(0, 0);//new Point(screen.Width / 4, screen.Height / 4);
            this.ClientSize = new Size(screen.Width - 15, screen.Height - 35);
            this.ShowScrollHeight = this.ClientSize.Height - HEADER_HEIGHT;

            ScrollBar = new VScrollBar();
            ScrollBar.Location = new Point(this.ClientSize.Width - ScrollBar.Width, HEADER_HEIGHT);
            ScrollBar.Height = this.ShowScrollHeight;
            ScrollBar.AutoSize = true;
            ScrollBar.Scroll += new ScrollEventHandler(Scroll_Move);

            bttnPanel = new ButtonPanel(new Point(0, MENU_HEIGHT), this.ClientSize);

            scrollPanel = new ScrollPanel(this);

            this.Controls.Add(ScrollBar);
            //this.Controls.Add(bttnPanel);
            this.Controls.Add(scrollPanel);
        }

        public void Window_Resize(object sender, EventArgs e) {
            if (bttnPanel != null) {
                ShowScrollHeight = this.ClientSize.Height - HEADER_HEIGHT;
                bttnPanel.ResizePanel(this.ClientSize);
                ScrollBar.Height = this.ClientSize.Height - HEADER_HEIGHT;
                scrollPanel.ResizeEvent = true;
                scrollPanel.Resize_Panel();
                ScrollBar.Location = new Point(this.ClientSize.Width - ScrollBar.Width, HEADER_HEIGHT);
            }
        }

        private void Scroll_Move(object sender, ScrollEventArgs e) {
            scrollPanel.Location = new Point(0, HEADER_HEIGHT-(ScrollBar.Value * SCROLL_FACTOR));
        }
        
        public int ShowScrollHeight { get; set; }
        public VScrollBar ScrollBar { get; set; }
        private ButtonPanel bttnPanel;
        private ScrollPanel scrollPanel;

        public const int HEADER_HEIGHT = 0;//MENU_HEIGHT + ButtonPanel.HEIGHT;
        public const int MENU_HEIGHT = 24;
        public const int SCROLL_FACTOR = 1;

        public static Window window;
    }
}
