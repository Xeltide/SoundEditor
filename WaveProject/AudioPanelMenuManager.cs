using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveProject.GUI;
using WaveProject.GUI.MenuItem;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject {
    class AudioPanelMenuManager {

        public AudioPanelMenuManager(AudioPanelMenu menu) {
            this.Menu = menu;

            this.Open = new GUI.MenuItem.File.OpenMenuItem(menu.GetEntry(), this);
            this.Close = new GUI.MenuItem.File.CloseMenuItem(menu.GetEntry(), this);
            
            this.Cut = new GUI.MenuItem.Edit.CutMenuItem(menu.GetEntry());
            this.Copy = new GUI.MenuItem.Edit.CopyMenuItem(menu.GetEntry());
            this.Paste = new GUI.MenuItem.Edit.PasteMenuItem(menu.GetEntry());
            this.Delete = new GUI.MenuItem.Edit.DeleteMenuItem(menu.GetEntry());

            this.Mono = new MonoMenuItem(menu.GetEntry(), this);
            this.Stereo = new StereoMenuItem(menu.GetEntry(), this);

            this.KHZ8 = new GUI.MenuItem.Encoding.khz8MenuItem(menu.GetEntry(), this);
            this.KHZ11 = new GUI.MenuItem.Encoding.khz11MenuItem(menu.GetEntry(), this);
            this.KHZ22 = new GUI.MenuItem.Encoding.khz22MenuItem(menu.GetEntry(), this);
            this.KHZ44 = new GUI.MenuItem.Encoding.khz44MenuItem(menu.GetEntry(), this);
            khz[0] = this.KHZ8;
            khz[1] = this.KHZ11;
            khz[2] = this.KHZ22;
            khz[3] = this.KHZ44;

            this.Bit8 = new GUI.MenuItem.Encoding.Bit8MenuItem(menu.GetEntry(), this);
            this.Bit16 = new GUI.MenuItem.Encoding.Bit16MenuItem(menu.GetEntry(), this);
            this.Bit24 = new GUI.MenuItem.Encoding.Bit24MenuItem(menu.GetEntry(), this);
            this.Bit32 = new GUI.MenuItem.Encoding.Bit32MenuItem(menu.GetEntry(), this);
            bits[0] = this.Bit8;
            bits[1] = this.Bit16;
            bits[2] = this.Bit24;
            bits[3] = this.Bit32;

            this.RunFourier = new GUI.MenuItem.Fourier.RunFourierMenuItem(menu.GetEntry(), this);
            this.ApplyFilter1 = new GUI.MenuItem.Fourier.ApplyFilter1MenuItem(menu.GetEntry(), this);
            this.ApplyFilter2 = new GUI.MenuItem.Fourier.ApplyFilter2MenuItem(menu.GetEntry(), this);
            this.ApplyFilter2.Enabled = false;
            this.Rect = new GUI.MenuItem.Windowing.RectangularWinMenuItem(menu.GetEntry(), this);
            this.Tri = new GUI.MenuItem.Windowing.TriangularWinMenuItem(menu.GetEntry(), this);
            this.Welch = new GUI.MenuItem.Windowing.WelchWinMenuItem(menu.GetEntry(), this);
            this.Sine = new GUI.MenuItem.Windowing.SineWinMenuItem(menu.GetEntry(), this);
        }

        public void Enable_Edit(bool enabled)
        {
            if (enabled)
            {
                Cut.Enabled = true;
                Copy.Enabled = true;
                Paste.Enabled = true;
                Delete.Enabled = true;
            } else
            {
                Cut.Enabled = false;
                Copy.Enabled = false;
                Paste.Enabled = false;
                Delete.Enabled = false;
            }
        }

        public void Mono_Click() {
            if (!Menu.IsMono) {
                Mono.Checked = true;
                Stereo.Checked = false;
                Menu.GetEntry().TimeChannel1.ChartArea.AxisX.Title = "Mono";
                Menu.GetEntry().LevelSlide2.Hide();
                Menu.GetEntry().TimeChannel2.Chart.Hide();
                Menu.GetEntry().FrequencyChannel2.Chart.Hide();

                Menu.IsMono = true;
                if (Menu.GetEntry().ParentRef is ScrollPanel) {
                    ((ScrollPanel)Menu.GetEntry().ParentRef).Resize_Panel();
                }
                Menu.GetEntry().PlayBar.Location = new Point(0, AudioPanel.MONO_HEIGHT - AudioPanel.PLAYBAR_HEIGHT);
                this.ApplyFilter2.Enabled = false;
            }
        }

        public void Stereo_Click() {
            if (Menu.IsMono && Menu.GetEntry().TimeChannel2 == null) {
                Console.WriteLine("Stereo");
                Mono.Checked = false;
                Stereo.Checked = true;
                Menu.GetEntry().TimeChannel1.ChartArea.AxisX.Title = "Left";
                Menu.GetEntry().TimeChannel2 = new AudioChannel("Right");
                Menu.GetEntry().TimeChannel2.Chart.SelectionRangeChanging += new EventHandler<CursorEventArgs>(Menu.GetEntry().SelectChange_2);
                Menu.GetEntry().TimeChannel2.Chart.Click += new EventHandler(Menu.GetEntry().SelectChange_2);
                Menu.GetEntry().TimeChannel2.Panel = Menu.GetEntry();
                Menu.GetEntry().FrequencyChannel2 = new FrequencyChannel("Frequency Filter");
                Menu.GetEntry().LevelSlide2 = new TrackBar();
                Menu.GetEntry().LevelSlide2.Orientation = Orientation.Vertical;
                Menu.GetEntry().LevelSlide2.Location = new Point(AudioPanel.SLIDER_WIDTH / 2, AudioPanel.MONO_HEIGHT - AudioPanel.PLAYBAR_HEIGHT);
                Menu.GetEntry().LevelSlide2.ClientSize = new Size(AudioPanel.SLIDER_WIDTH, AudioChannel.HEIGHT);

                Menu.GetEntry().Controls.Add(Menu.GetEntry().LevelSlide2);
                Menu.GetEntry().Controls.Add(Menu.GetEntry().TimeChannel2.Chart);
                Menu.GetEntry().Controls.Add(Menu.GetEntry().FrequencyChannel2.Chart);

                Menu.GetEntry().Menu.IsMono = false;
                if (Menu.GetEntry().ParentRef is ScrollPanel) {
                    ((ScrollPanel)Menu.GetEntry().ParentRef).Resize_Panel();
                }
                this.ApplyFilter2.Enabled = true;
            } else if (Menu.GetEntry().Menu.IsMono && Menu.GetEntry().TimeChannel2 != null) {
                Mono.Checked = false;
                Stereo.Checked = true;
                Menu.GetEntry().LevelSlide2.Show();
                Menu.GetEntry().TimeChannel2.Chart.Show();
                Menu.GetEntry().FrequencyChannel2.Chart.Show();

                Menu.GetEntry().Menu.IsMono = false;
                if (Menu.GetEntry().ParentRef is ScrollPanel) {
                    ((ScrollPanel)Menu.GetEntry().ParentRef).Resize_Panel();
                }
                this.ApplyFilter2.Enabled = true;
            }
            Menu.GetEntry().PlayBar.Location = new Point(0, AudioPanel.STEREO_HEIGHT - AudioPanel.PLAYBAR_HEIGHT);
        }

        public void SetKHZ(int i)
        {
            khz[checkedKHZ].Checked = false;
            checkedKHZ = i;
            khz[checkedKHZ].Checked = true;
        }

        public void SetBits(int i)
        {
            bits[checkedBits].Checked = false;
            checkedBits = i;
            bits[checkedBits].Checked = true;
        }

        public void Open_Click() {
            switch (Menu.GetEntry().PanelData.RIFFData.BitsPerSample) {
                case 8:
                    BitsPerSample = 8;
                    Bit8.Checked = true;
                    Bit16.Checked = false;
                    Bit24.Checked = false;
                    Bit32.Checked = false;
                    Bit8.Enabled = true;
                    Bit16.Enabled = false;
                    Bit24.Enabled = false;
                    Bit32.Enabled = false;
                    checkedBits = 0;
                    break;
                case 16:
                    BitsPerSample = 16;
                    Bit8.Checked = false;
                    Bit16.Checked = true;
                    Bit24.Checked = false;
                    Bit32.Checked = false;
                    Bit8.Enabled = false;
                    Bit16.Enabled = true;
                    Bit24.Enabled = false;
                    Bit32.Enabled = false;
                    checkedBits = 1;
                    break;
                case 24:
                    BitsPerSample = 24;
                    Bit8.Checked = false;
                    Bit16.Checked = false;
                    Bit24.Checked = true;
                    Bit32.Checked = false;
                    Bit8.Enabled = false;
                    Bit16.Enabled = false;
                    Bit24.Enabled = true;
                    Bit32.Enabled = false;
                    checkedBits = 2;
                    break;
                case 32:
                    BitsPerSample = 32;
                    Bit8.Checked = false;
                    Bit16.Checked = false;
                    Bit24.Checked = false;
                    Bit32.Checked = true;
                    Bit8.Enabled = false;
                    Bit16.Enabled = false;
                    Bit24.Enabled = false;
                    Bit32.Enabled = true;
                    checkedBits = 3;
                    break;
            }

            switch (Menu.GetEntry().PanelData.RIFFData.SampleRate) {
                case 8000:
                    SampleRate = 8000;
                    KHZ8.Checked = true;
                    KHZ11.Checked = false;
                    KHZ22.Checked = false;
                    KHZ44.Checked = false;
                    KHZ8.Enabled = true;
                    KHZ11.Enabled = false;
                    KHZ22.Enabled = false;
                    KHZ44.Enabled = false;
                    checkedKHZ = 0;
                    break;
                case 11025:
                    SampleRate = 11025;
                    KHZ8.Checked = false;
                    KHZ11.Checked = true;
                    KHZ22.Checked = false;
                    KHZ44.Checked = false;
                    KHZ8.Enabled = false;
                    KHZ11.Enabled = true;
                    KHZ22.Enabled = false;
                    KHZ44.Enabled = false;
                    checkedKHZ = 1;
                    break;
                case 22050:
                    SampleRate = 22050;
                    KHZ8.Checked = false;
                    KHZ11.Checked = false;
                    KHZ22.Checked = true;
                    KHZ44.Checked = false;
                    KHZ8.Enabled = false;
                    KHZ11.Enabled = false;
                    KHZ22.Enabled = true;
                    KHZ44.Enabled = false;
                    checkedKHZ = 2;
                    break;
                case 44100:
                    SampleRate = 44100;
                    KHZ8.Checked = false;
                    KHZ11.Checked = false;
                    KHZ22.Checked = false;
                    KHZ44.Checked = true;
                    KHZ8.Enabled = false;
                    KHZ11.Enabled = false;
                    KHZ22.Enabled = false;
                    KHZ44.Enabled = true;
                    checkedKHZ = 3;
                    break;
            }
        }

        public GUI.MenuItem.File.OpenMenuItem Open { get; set; }
        public GUI.MenuItem.File.CloseMenuItem Close { get; set; }

        public GUI.MenuItem.Edit.CutMenuItem Cut { get; set; }
        public GUI.MenuItem.Edit.CopyMenuItem Copy { get; set; }
        public GUI.MenuItem.Edit.PasteMenuItem Paste { get; set; }
        public GUI.MenuItem.Edit.DeleteMenuItem Delete { get; set; }

        public MonoMenuItem Mono { get; set; }
        public StereoMenuItem Stereo { get; set; }

        public Abstract.AbsMenuItem<int>[] khz = new Abstract.AbsMenuItem<int>[4];
        public int checkedKHZ = 0;
        public GUI.MenuItem.Encoding.khz8MenuItem KHZ8 { get; set; }
        public GUI.MenuItem.Encoding.khz11MenuItem KHZ11 { get; set; }
        public GUI.MenuItem.Encoding.khz22MenuItem KHZ22 { get; set; }
        public GUI.MenuItem.Encoding.khz44MenuItem KHZ44 { get; set; }

        public Abstract.AbsMenuItem<short>[] bits = new Abstract.AbsMenuItem<short>[4];
        public int checkedBits = 0;
        public GUI.MenuItem.Encoding.Bit8MenuItem Bit8 { get; set; }
        public GUI.MenuItem.Encoding.Bit16MenuItem Bit16 { get; set; }
        public GUI.MenuItem.Encoding.Bit24MenuItem Bit24 { get; set; }
        public GUI.MenuItem.Encoding.Bit32MenuItem Bit32 { get; set; }
        public int SampleRate { get; set; }
        public short BitsPerSample { get; set; }

        public GUI.MenuItem.Fourier.RunFourierMenuItem RunFourier { get; set; }
        public GUI.MenuItem.Fourier.ApplyFilter1MenuItem ApplyFilter1 { get; set; }
        public GUI.MenuItem.Fourier.ApplyFilter2MenuItem ApplyFilter2 { get; set; }
        public GUI.MenuItem.Windowing.RectangularWinMenuItem Rect { get; set; }
        public GUI.MenuItem.Windowing.TriangularWinMenuItem Tri { get; set; }
        public GUI.MenuItem.Windowing.WelchWinMenuItem Welch { get; set; }
        public GUI.MenuItem.Windowing.SineWinMenuItem Sine { get; set; }
        public Structure.TypeKey.Windowing WindowMode { get; set; }

        public AudioPanelMenu Menu;
    }
}
