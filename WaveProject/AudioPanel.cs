using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using WaveProject.GUI;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject {
    class AudioPanel : Panel {

        public AudioPanel(ScrollPanel parent, Point position) {
            this.ParentRef = parent;
            this.BackColor = Color.FromArgb(255, 25, 25, 25);
            this.MinimumSize = new Size(0, 240);
            this.ClientSize = new Size(ParentRef.Width - (2 * PADDING), AudioPanel.MONO_HEIGHT - (2 * PADDING));
            this.Location = new Point(position.X + PADDING, position.Y + (PADDING / 2));

            Init(position);
        }

        private void Init(Point position) {
            this.PanelData = new AudioPanelData();
            this.Menu = new AudioPanelMenu(this);

            LevelSlide1 = new TrackBar();
            LevelSlide1.Orientation = Orientation.Vertical;
            LevelSlide1.Location = new Point(SLIDER_WIDTH / 2, MENU_HEIGHT);
            LevelSlide1.ClientSize = new Size(SLIDER_WIDTH, AudioChannel.HEIGHT);

            TimeChannel1 = new AudioChannel("Mono", SLIDER_WIDTH, MENU_HEIGHT, (ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, ParentRef.Height);
            FrequencyChannel1 = new FrequencyChannel("Frequency Filter", TimeChannel1.Chart.ClientSize.Width + SLIDER_WIDTH, MENU_HEIGHT, TimeChannel1.Chart.ClientSize.Width, FrequencyChannel.SIZE);

            TimeChannel1.Panel = this;
            TimeChannel1.Chart.Click += new EventHandler(SelectChange_1);
            TimeChannel1.Chart.SelectionRangeChanging += new EventHandler<CursorEventArgs>(SelectChange_1);

            PlayBar = new AP_PlayBar(this, 0, MONO_HEIGHT - PLAYBAR_HEIGHT, ParentRef.Width, PLAYBAR_HEIGHT);
            this.Controls.Add(Menu);
            this.Controls.Add(LevelSlide1);
            this.Controls.Add(TimeChannel1.Chart);
            this.Controls.Add(FrequencyChannel1.Chart);
            this.Controls.Add(PlayBar);
        }

        public void Resize_Panel() {
            if (!Menu.IsMono) {
                this.ClientSize = new Size(ParentRef.Width - (2 * PADDING), STEREO_HEIGHT);
                
                TimeChannel2.Chart.Location = new Point(TimeChannel1.Chart.Location.X, MONO_HEIGHT - PLAYBAR_HEIGHT);
                if (Menu.IsFrequencyShown) {
                    TimeChannel2.Resize_Chart(new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
                    FrequencyChannel2.Resize_Chart(new Point(((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2) + SLIDER_WIDTH, MONO_HEIGHT - PLAYBAR_HEIGHT), new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
                } else {
                    TimeChannel2.Resize_Chart(new Size(ParentRef.Width - SLIDER_WIDTH - 30, AudioChannel.HEIGHT));
                }
            } else {
                this.ClientSize = new Size(ParentRef.Width - (2 * PADDING), MONO_HEIGHT);
            }
            

            if (Menu.IsFrequencyShown) {
                TimeChannel1.Resize_Chart(new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
            } else {
                TimeChannel1.Resize_Chart(new Size(ParentRef.Width - SLIDER_WIDTH - 30, ParentRef.Height));
            }
            FrequencyChannel1.Resize_Chart(new Point(((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2) + SLIDER_WIDTH, FrequencyChannel1.Chart.Location.Y), new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
            PlayBar.ClientSize = new Size(ParentRef.Width, PLAYBAR_HEIGHT);
        }

        public void ClearAllCharts() {
            TimeChannel1.Series.Points.Clear();
            FrequencyChannel1.Series.Points.Clear();
            if (TimeChannel2 != null) {
                TimeChannel2.Series.Points.Clear();
                FrequencyChannel2.Series.Points.Clear();
            }
        }

        public void SelectChange_1(object sender, EventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Cursor moving = TimeChannel1.ChartArea.CursorX;
            Console.WriteLine(moving.Position);
            if (TimeChannel2 != null)
            {
                TimeChannel2.ChartArea.CursorX.Position = moving.Position;
                TimeChannel2.ChartArea.CursorX.SelectionStart = moving.SelectionStart;
                TimeChannel2.ChartArea.CursorX.SelectionEnd = moving.SelectionEnd;
            }
        }

        public void SelectChange_2(object sender, EventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Cursor moving = TimeChannel2.ChartArea.CursorX;
            TimeChannel1.ChartArea.CursorX.Position = moving.Position;
            TimeChannel1.ChartArea.CursorX.SelectionStart = moving.SelectionStart;
            TimeChannel1.ChartArea.CursorX.SelectionEnd = moving.SelectionEnd;
        }

        public AudioPanelMenu Menu { get; set; }

        public TrackBar LevelSlide1 { get; set; }
        public TrackBar LevelSlide2 { get; set; }
        public AudioChannel TimeChannel1 { get; set; }
        public AudioChannel TimeChannel2 { get; set; }
        public FrequencyChannel FrequencyChannel1 { get; set; }
        public FrequencyChannel FrequencyChannel2 { get; set; }
        public AP_PlayBar PlayBar { get; set; }

        public ScrollPanel ParentRef { get; set; }
        public const int PADDING = 12;
        public const int MENU_HEIGHT = 24;
        public const int SLIDER_WIDTH = 60;
        public const int PLAYBAR_HEIGHT = 60;
        public const int MONO_HEIGHT = MENU_HEIGHT + AudioChannel.HEIGHT + PLAYBAR_HEIGHT;
        public const int STEREO_HEIGHT = MONO_HEIGHT + AudioChannel.HEIGHT;

        public AudioPanelData PanelData { get; set; }
    }
}
