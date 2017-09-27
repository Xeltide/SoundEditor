using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WaveProject {
    class AudioPanelMenu : MenuStrip {

        public AudioPanelMenu(AudioPanel parent) {
            this.parent = parent;

            InitChildren();

            this.BackColor = Color.FromArgb(255, 35, 35, 35);
            this.Items.AddRange(new ToolStripItem[] {
            this.file,
            this.edit,
            this.record,
            this.encode,
            this.toggleFreq});
            this.Location = new Point(0, 0);
            this.Name = "Menu";
            this.Size = new Size(this.ClientSize.Width, AudioPanel.MENU_HEIGHT);
            this.TabIndex = 0;
            this.Text = "Menu";
            this.Renderer = new ToolStripProfessionalRenderer(new MenuColoursTable());

            IsMono = true;
            IsFrequencyShown = true;
        }

        private void InitChildren() {
            file = new ToolStripMenuItem();
            file.ForeColor = Color.White;
            file.Text = "File";
            open = new ToolStripMenuItem();
            open.ForeColor = Color.White;
            open.Text = "Open";
            open.Click += new EventHandler(Open_Click);
            saveAs = new ToolStripMenuItem();
            saveAs.ForeColor = Color.White;
            saveAs.Text = "Save As ...";
            saveAs.Click += new EventHandler(SaveAs_Click);
            file.DropDownItems.AddRange(new ToolStripItem[] {
            this.open,
            this.saveAs});

            edit = new ToolStripMenuItem();
            edit.ForeColor = Color.White;
            edit.Text = "Edit";

            record = new ToolStripMenuItem();
            record.ForeColor = Color.White;
            record.Text = "Record";

            encode = new ToolStripMenuItem();
            encode.ForeColor = Color.White;
            encode.Text = "Encoding";
            mono = new ToolStripMenuItem();
            mono.ForeColor = Color.White;
            mono.Text = "Mono";
            mono.Checked = true;
            mono.Click += new EventHandler(Mono_Click);
            stereo = new ToolStripMenuItem();
            stereo.ForeColor = Color.White;
            stereo.Text = "Stereo";
            stereo.Click += new EventHandler(Stereo_Click);
            sep = new ToolStripSeparator();
            khz8 = new ToolStripMenuItem();
            khz8.ForeColor = Color.White;
            khz8.Text = "8,000 Hz";
            khz8.Checked = true;
            khz11 = new ToolStripMenuItem();
            khz11.ForeColor = Color.White;
            khz11.Text = "11,025 Hz";
            khz22 = new ToolStripMenuItem();
            khz22.ForeColor = Color.White;
            khz22.Text = "22,050 Hz";
            khz44 = new ToolStripMenuItem();
            khz44.ForeColor = Color.White;
            khz44.Text = "44,100 Hz";
            sep2 = new ToolStripSeparator();
            bit8 = new ToolStripMenuItem();
            bit8.ForeColor = Color.White;
            bit8.Text = "8 bit";
            bit8.Checked = true;
            bit16 = new ToolStripMenuItem();
            bit16.ForeColor = Color.White;
            bit16.Text = "16 bit";
            bit24 = new ToolStripMenuItem();
            bit24.ForeColor = Color.White;
            bit24.Text = "24 bit";
            bit32 = new ToolStripMenuItem();
            bit32.ForeColor = Color.White;
            bit32.Text = "32 bit";
            encode.DropDownItems.AddRange(new ToolStripItem[] {
            this.mono,
            this.stereo,
            this.sep,
            this.khz8,
            this.khz11,
            this.khz22,
            this.khz44,
            this.sep2,
            this.bit8,
            this.bit16,
            this.bit24,
            this.bit32});

            toggleFreq = new ToolStripMenuItem();
            toggleFreq.Text = "";
            toggleFreq.Alignment = ToolStripItemAlignment.Right;
            toggleFreq.Image = new Bitmap("C:\\Users\\Xeltide\\Desktop\\Archive\\BCIT\\Term 3\\COMP 3931\\Project\\WaveProject\\WaveProject\\View.png");
            toggleFreq.ImageScaling = ToolStripItemImageScaling.None;
            toggleFreq.Click += new EventHandler(Freq_Toggle);
        }
        
        private void Open_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = @"C:\";
            ofd.Title = "Browse WAV Files";

            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            ofd.DefaultExt = "wav";
            ofd.Filter = "WAV files (*.wav) | *.wav";
            ofd.RestoreDirectory = true;

            ofd.ReadOnlyChecked = true;
            ofd.ShowReadOnly = true;

            if (ofd.ShowDialog() == DialogResult.OK) {
                parent.PanelData.SetFilePath(ofd.FileName);
                parent.PanelData.SplitWavData();
                parent.ClearAllCharts();
                parent.LoadTimeChart(1, parent.PanelData.GetReadableChannel(0));
                System.Diagnostics.Debug.WriteLine("Started thread 1");
                LoadFrequencyChart_Thread();

                if (parent.PanelData.RawChannelData.Count > 1) {
                    this.Stereo_Click(null, EventArgs.Empty);
                    mono.Enabled = false;
                    parent.LoadTimeChart(2, parent.PanelData.GetReadableChannel(1));
                    System.Diagnostics.Debug.WriteLine("Started thread 2");
                    LoadFrequencyChart_Thread();
                } else {
                    this.Mono_Click(null, EventArgs.Empty);
                    stereo.Enabled = false;
                }

                switch (parent.PanelData.RIFFData.BitsPerSample) {
                    case 8:
                        bit8.Checked = true;
                        bit16.Enabled = false;
                        bit24.Enabled = false;
                        bit32.Enabled = false;
                        break;
                    case 16:
                        bit8.Enabled = false;
                        bit16.Checked = true;
                        bit24.Enabled = false;
                        bit32.Enabled = false;
                        break;
                    case 24:
                        bit8.Enabled = false;
                        bit16.Enabled = false;
                        bit24.Checked = true;
                        bit32.Enabled = false;
                        break;
                    case 32:
                        bit8.Enabled = false;
                        bit16.Enabled = false;
                        bit24.Enabled = false;
                        bit32.Checked = true;
                        break;
                    default:
                        bit8.Enabled = false;
                        bit16.Enabled = false;
                        bit24.Enabled = false;
                        bit32.Enabled = false;
                        break;
                }

                switch (parent.PanelData.RIFFData.SampleRate) {
                    case 8000:
                        khz8.Checked = true;
                        khz11.Enabled = false;
                        khz22.Enabled = false;
                        khz44.Enabled = false;
                        break;
                    case 11025:
                        khz8.Enabled = false;
                        khz11.Checked = true;
                        khz22.Enabled = false;
                        khz44.Enabled = false;
                        break;
                    case 22050:
                        khz8.Enabled = false;
                        khz11.Enabled = false;
                        khz22.Checked = true;
                        khz44.Enabled = false;
                        break;
                    case 44100:
                        khz8.Enabled = false;
                        khz11.Enabled = false;
                        khz22.Enabled = false;
                        khz44.Checked = true;
                        break;
                    default:
                        khz8.Enabled = false;
                        khz11.Enabled = false;
                        khz22.Enabled = false;
                        khz44.Enabled = false;
                        break;
                }
            }
        }

        private void SaveAs_Click(object sender, EventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.InitialDirectory = @"C:\";
            sfd.Title = "Save As ...";

            sfd.CheckPathExists = true;

            sfd.AddExtension = true;
            sfd.DefaultExt = "wav";
            sfd.Filter = "WAV files (*.wav) | *.wav";
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK) {
                WavPackager wp = new WavPackager(sfd.FileName);
                wp.WriteFile(parent.PanelData.RIFFData);
            }
        }

        private void LoadFrequencyChart_Thread() {
            System.Threading.Thread worker = new System.Threading.Thread(() => {
                List<ComplexNumber> output = FourierMath.DFTIntegers(parent.PanelData.GetReadableChannel(0), parent.PanelData.RIFFData);
                Invoke(new MethodInvoker(delegate {
                    parent.LoadFrequencyChart(0, output);
                }));
            });
            worker.Start();
        }

        protected void Mono_Click(object sender, EventArgs e) {
            if (!IsMono) {
                mono.Checked = true;
                stereo.Checked = false;
                parent.LevelSlide2.Hide();
                parent.TimeChannel2.Chart.Hide();
                parent.FrequencyChannel2.Chart.Hide();

                IsMono = true;
                if (parent.ParentRef is ScrollPanel) {
                    ((ScrollPanel)parent.ParentRef).Resize_Panel();
                }
            }
        }

        protected void Stereo_Click(object sender, EventArgs e) {
            if (IsMono && parent.TimeChannel2 == null) {
                mono.Checked = false;
                stereo.Checked = true;
                parent.TimeChannel2 = new AudioChannel("Right");
                parent.FrequencyChannel2 = new FrequencyChannel("Frequency Filter");
                parent.LevelSlide2 = new TrackBar();
                parent.LevelSlide2.Orientation = Orientation.Vertical;
                parent.LevelSlide2.Location = new Point(AudioPanel.SLIDER_WIDTH / 2, AudioPanel.MONO_HEIGHT);
                parent.LevelSlide2.ClientSize = new Size(AudioPanel.SLIDER_WIDTH, AudioPanel.MONO_HEIGHT - AudioPanel.MENU_HEIGHT - AudioPanel.PADDING);

                parent.Controls.Add(parent.LevelSlide2);
                parent.Controls.Add(parent.TimeChannel2.Chart);
                parent.Controls.Add(parent.FrequencyChannel2.Chart);

                IsMono = false;
                if (parent.ParentRef is ScrollPanel) {
                    ((ScrollPanel)parent.ParentRef).Resize_Panel();
                }
            } else if (IsMono && parent.TimeChannel2 != null) {
                mono.Checked = false;
                stereo.Checked = true;
                parent.LevelSlide2.Show();
                parent.TimeChannel2.Chart.Show();
                parent.FrequencyChannel2.Chart.Show();

                IsMono = false;
                if (parent.ParentRef is ScrollPanel) {
                    ((ScrollPanel)parent.ParentRef).Resize_Panel();
                }
            }
        }
        
        protected void Freq_Toggle(object sender, EventArgs e) {
            if (IsFrequencyShown) {
                parent.FrequencyChannel1.Chart.Hide();
                if (!IsMono && parent.FrequencyChannel2 != null) {
                    parent.FrequencyChannel2.Chart.Hide();
                }
            } else {
                parent.FrequencyChannel1.Chart.Show();
                if (!IsMono && parent.FrequencyChannel2 != null) {
                    parent.FrequencyChannel2.Chart.Show();
                }
            }
            IsFrequencyShown = !IsFrequencyShown;
            parent.Resize_Panel();
        }

        private ToolStripMenuItem file;
        private ToolStripMenuItem open;
        private ToolStripMenuItem saveAs;

        private ToolStripMenuItem edit;

        private ToolStripMenuItem record;

        private ToolStripMenuItem encode;
        private ToolStripMenuItem mono;
        private ToolStripMenuItem stereo;
        private ToolStripSeparator sep;
        private ToolStripMenuItem khz8;
        private ToolStripMenuItem khz11;
        private ToolStripMenuItem khz22;
        private ToolStripMenuItem khz44;
        private ToolStripSeparator sep2;
        private ToolStripMenuItem bit8;
        private ToolStripMenuItem bit16;
        private ToolStripMenuItem bit24;
        private ToolStripMenuItem bit32;
        public bool IsMono { get; set; }

        private ToolStripMenuItem toggleFreq;
        public bool IsFrequencyShown { get; set; }

        private AudioPanel parent;
    }
}
