using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using WaveProject.GUI;

namespace WaveProject.GUI {
    class AudioPanelMenu : MenuStrip {

        public AudioPanelMenu(AudioPanel parent) {
            this.parent = parent;
            this.manager = new AudioPanelMenuManager(this);

            InitChildren();

            this.BackColor = Color.FromArgb(255, 35, 35, 35);
            this.Items.AddRange(new ToolStripItem[] {
            this.file,
            this.edit,
            this.encode,
            this.fourier,
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
            saveAs = new ToolStripMenuItem();
            saveAs.ForeColor = Color.White;
            saveAs.Text = "Save As ...";
            saveAs.Click += new EventHandler(SaveAs_Click);
            file.DropDownItems.AddRange(new ToolStripItem[] {
            manager.Open,
            this.saveAs,
            new ToolStripSeparator(),
            manager.Close});

            edit = new ToolStripMenuItem();
            edit.ForeColor = Color.White;
            edit.Text = "Edit";
            edit.DropDownItems.AddRange(new ToolStripItem[] {
                manager.Cut,
                manager.Copy,
                manager.Paste,
                manager.Delete
            });

            encode = new ToolStripMenuItem();
            encode.ForeColor = Color.White;
            encode.Text = "Encoding";
            encode.DropDownItems.AddRange(new ToolStripItem[] {
            manager.Mono,
            manager.Stereo,
            new ToolStripSeparator(),
            manager.KHZ8,
            manager.KHZ11,
            manager.KHZ22,
            manager.KHZ44,
            new ToolStripSeparator(),
            manager.Bit8,
            manager.Bit16,
            manager.Bit24,
            manager.Bit32});

            fourier = new ToolStripMenuItem();
            fourier.ForeColor = Color.White;
            fourier.Text = "Fourier";
            fourier.DropDownItems.AddRange(new ToolStripItem[] {
                manager.RunFourier,
                manager.ApplyFilter1,
                manager.ApplyFilter2,
                new ToolStripSeparator(),
                manager.Rect,
                manager.Tri,
                manager.Welch,
                manager.Sine
            });

            toggleFreq = new ToolStripMenuItem();
            toggleFreq.Text = "";
            toggleFreq.Alignment = ToolStripItemAlignment.Right;
            toggleFreq.Image = new Bitmap("C:\\Users\\Xeltide\\Desktop\\Archive\\BCIT\\Term 3\\COMP 3931\\Project\\WaveProject\\WaveProject\\View.png");
            toggleFreq.ImageScaling = ToolStripItemImageScaling.None;
            toggleFreq.Click += new EventHandler(Freq_Toggle);
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
                //wp.WriteFile(parent.PanelData.RIFFData);
                wp.WriteFile(wp.GenerateRIFFData(parent));
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

        public AudioPanel GetEntry() {
            return parent;
        }

        private ToolStripMenuItem file;
        private ToolStripMenuItem saveAs;

        private ToolStripMenuItem edit;

        private ToolStripMenuItem encode;
        public bool IsMono { get; set; }

        private ToolStripMenuItem fourier;

        private ToolStripMenuItem toggleFreq;
        public bool IsFrequencyShown { get; set; }

        private AudioPanel parent;
        public AudioPanelMenuManager manager;
    }
}
