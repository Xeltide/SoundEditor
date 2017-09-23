using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WaveProject {
    class AudioPanel : Panel {

        public AudioPanel(ScrollableControl parent, Point position) {
            this.ParentRef = parent;
            this.BackColor = Color.FromArgb(255, 25, 25, 25);
            this.MinimumSize = new Size(0, 240);
            this.ClientSize = new Size(ParentRef.Width - (2 * PADDING), AudioPanel.MONO_HEIGHT - (2 * PADDING));
            this.Location = new Point(position.X + PADDING, position.Y + (PADDING / 2));

            Init(position);
        }

        private void Init(Point position) {
            file = new ToolStripMenuItem();
            file.Text = "File";

            edit = new ToolStripMenuItem();
            edit.Text = "Edit";

            record = new ToolStripMenuItem();
            record.Text = "Record";

            encode = new ToolStripMenuItem();
            encode.Text = "Encoding";
            mono = new ToolStripMenuItem();
            mono.Text = "Mono";
            mono.Click += new EventHandler(Mono_Click);
            stereo = new ToolStripMenuItem();
            stereo.Text = "Stereo";
            stereo.Click += new EventHandler(Stereo_Click);
            encode.DropDownItems.AddRange(new ToolStripItem[] {
            this.mono,
            this.stereo});

            toggleFreq = new ToolStripMenuItem();
            toggleFreq.Text = "";
            toggleFreq.Alignment = ToolStripItemAlignment.Right;
            toggleFreq.Image = new Bitmap("C:\\Users\\Xeltide\\Desktop\\Archive\\BCIT\\Term 3\\COMP 3931\\Project\\WaveProject\\WaveProject\\View.png");
            toggleFreq.ImageScaling = ToolStripItemImageScaling.None;
            toggleFreq.Click += new EventHandler(Freq_Toggle);

            menu = new MenuStrip();
            this.menu.BackColor = Color.FromArgb(255, 30, 30, 30);
            this.menu.Items.AddRange(new ToolStripItem[] {
            this.file,
            this.edit,
            this.record,
            this.encode,
            this.toggleFreq});
            this.menu.Location = new Point(0, 0);
            this.menu.Name = "Menu";
            this.menu.Size = new Size(this.ClientSize.Width, MENU_HEIGHT);
            this.menu.TabIndex = 0;
            this.menu.Text = "Menu";

            levelSlide = new TrackBar();
            levelSlide.Orientation = Orientation.Vertical;
            levelSlide.Location = new Point(SLIDER_WIDTH / 2, MENU_HEIGHT);
            levelSlide.ClientSize = new Size(SLIDER_WIDTH, MONO_HEIGHT - MENU_HEIGHT - PADDING);

            ch1 = new AudioChannel("Mono", SLIDER_WIDTH, MENU_HEIGHT, (ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, ParentRef.Height);

            freq1 = new FrequencyChannel("Frequency Filter", ch1.Chart.ClientSize.Width + SLIDER_WIDTH, MENU_HEIGHT, ch1.Chart.ClientSize.Width, FrequencyChannel.SIZE);

            this.Controls.Add(menu);
            this.Controls.Add(levelSlide);
            this.Controls.Add(ch1.Chart);
            this.Controls.Add(freq1.Chart);
        }

        public void Resize_Panel() {
            if (!isMono) {
                this.ClientSize = new Size(ParentRef.Width - (2 * PADDING), STEREO_HEIGHT);
                
                ch2.Chart.Location = new Point(ch1.Chart.Location.X, MONO_HEIGHT);
                if (freqOn) {
                    ch2.Resize_Chart(new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
                    freq2.Resize_Chart(new Point(((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2) + SLIDER_WIDTH, MONO_HEIGHT), new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
                } else {
                    ch2.Resize_Chart(new Size(ParentRef.Width - SLIDER_WIDTH, AudioChannel.HEIGHT));
                }
            } else {
                this.ClientSize = new Size(ParentRef.Width - (2 * PADDING), MONO_HEIGHT);
            }
            

            if (freqOn) {
                ch1.Resize_Chart(new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
            } else {
                ch1.Resize_Chart(new Size(ParentRef.Width - SLIDER_WIDTH, ParentRef.Height));
            }
            freq1.Resize_Chart(new Point(((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2) + SLIDER_WIDTH, freq1.Chart.Location.Y), new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
        }

        protected void Freq_Toggle(object sender, EventArgs e) {
            if (freqOn) {
                freq1.Chart.Hide();
                if (!isMono && freq2 != null) {
                    freq2.Chart.Hide();
                }
            } else {
                freq1.Chart.Show();
                if (!isMono && freq2 != null) {
                    freq2.Chart.Show();
                }
            }
            freqOn = !freqOn;
            Resize_Panel();
        }

        protected void Mono_Click(object sender, EventArgs e) {
            if (!isMono) {
                levelSlide2.Hide();
                ch2.Chart.Hide();
                freq2.Chart.Hide();
            }

            isMono = true;
            if (ParentRef is ScrollPanel) {
                ((ScrollPanel)ParentRef).Resize_Panel();
            }
        }

        protected void Stereo_Click(object sender, EventArgs e) {
            if (isMono && ch2 == null) {
                ch2 = new AudioChannel("Right");
                freq2 = new FrequencyChannel("Frequency Filter");
                levelSlide2 = new TrackBar();
                levelSlide2.Orientation = Orientation.Vertical;
                levelSlide2.Location = new Point(SLIDER_WIDTH / 2, MONO_HEIGHT);
                levelSlide2.ClientSize = new Size(SLIDER_WIDTH, MONO_HEIGHT - MENU_HEIGHT - PADDING);

                this.Controls.Add(levelSlide2);
                this.Controls.Add(ch2.Chart);
                this.Controls.Add(freq2.Chart);
                
                // Load in current byte array in different format
            } else if (isMono && ch2 != null) {
                levelSlide2.Show();
                ch2.Chart.Show();
                freq2.Chart.Show();
                
                // Load in current byte array in different format
            }

            isMono = false;
            if (ParentRef is ScrollPanel) {
                ((ScrollPanel)ParentRef).Resize_Panel();
            }
        }

        private MenuStrip menu;
        private ToolStripMenuItem file;

        private ToolStripMenuItem edit;

        private ToolStripMenuItem record;

        private ToolStripMenuItem encode;
        private ToolStripMenuItem mono;
        private ToolStripMenuItem stereo;
        private bool isMono = true;

        private ToolStripMenuItem toggleFreq;
        private bool freqOn = true;

        private TrackBar levelSlide;
        private TrackBar levelSlide2;
        private AudioChannel ch1;
        private AudioChannel ch2;
        private FrequencyChannel freq1;
        private FrequencyChannel freq2;
        
        public bool IsMono { get { return isMono; } }
        public ScrollableControl ParentRef { get; set; }
        public const int PADDING = 12;
        public const int MENU_HEIGHT = 24;
        public const int SLIDER_WIDTH = 60;
        public const int MONO_HEIGHT = 240;
        public const int STEREO_HEIGHT = MONO_HEIGHT + AudioChannel.HEIGHT;
    }
}
