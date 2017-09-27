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
            this.PanelData = new AudioPanelData();
            this.Menu = new AudioPanelMenu(this);

            LevelSlide1 = new TrackBar();
            LevelSlide1.Orientation = Orientation.Vertical;
            LevelSlide1.Location = new Point(SLIDER_WIDTH / 2, MENU_HEIGHT);
            LevelSlide1.ClientSize = new Size(SLIDER_WIDTH, MONO_HEIGHT - MENU_HEIGHT - PADDING);

            TimeChannel1 = new AudioChannel("Mono", SLIDER_WIDTH, MENU_HEIGHT, (ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, ParentRef.Height);
            FrequencyChannel1 = new FrequencyChannel("Frequency Filter", TimeChannel1.Chart.ClientSize.Width + SLIDER_WIDTH, MENU_HEIGHT, TimeChannel1.Chart.ClientSize.Width, FrequencyChannel.SIZE);

            this.Controls.Add(Menu);
            this.Controls.Add(LevelSlide1);
            this.Controls.Add(TimeChannel1.Chart);
            this.Controls.Add(FrequencyChannel1.Chart);
        }

        public void Resize_Panel() {
            if (!Menu.IsMono) {
                this.ClientSize = new Size(ParentRef.Width - (2 * PADDING), STEREO_HEIGHT);
                
                TimeChannel2.Chart.Location = new Point(TimeChannel1.Chart.Location.X, MONO_HEIGHT);
                if (Menu.IsFrequencyShown) {
                    TimeChannel2.Resize_Chart(new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
                    FrequencyChannel2.Resize_Chart(new Point(((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2) + SLIDER_WIDTH, MONO_HEIGHT), new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
                } else {
                    TimeChannel2.Resize_Chart(new Size(ParentRef.Width - SLIDER_WIDTH, AudioChannel.HEIGHT));
                }
            } else {
                this.ClientSize = new Size(ParentRef.Width - (2 * PADDING), MONO_HEIGHT);
            }
            

            if (Menu.IsFrequencyShown) {
                TimeChannel1.Resize_Chart(new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
            } else {
                TimeChannel1.Resize_Chart(new Size(ParentRef.Width - SLIDER_WIDTH, ParentRef.Height));
            }
            FrequencyChannel1.Resize_Chart(new Point(((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2) + SLIDER_WIDTH, FrequencyChannel1.Chart.Location.Y), new Size((ParentRef.Width - SLIDER_WIDTH - FrequencyChannel.WIDTH_PADDING) / 2, AudioChannel.HEIGHT));
        }

        public void ClearAllCharts() {
            TimeChannel1.Series.Points.Clear();
            FrequencyChannel1.Series.Points.Clear();
            if (TimeChannel2 != null) {
                TimeChannel2.Series.Points.Clear();
                FrequencyChannel2.Series.Points.Clear();
            }
        }

        public void LoadTimeChart(int number, List<int> input) {
            AudioChannel load;
            if (number == 2) {
                load = TimeChannel2;
            } else {
                load = TimeChannel1;
            }
            double step = 1.0 / PanelData.RIFFData.SampleRate;
            DateTime dt = DateTime.Today;
            
            // Too slow to load in all the data points. Potentially create culling regions and dynamically load them
            for (int i = 0; i < input.Count; i++) {
                DateTime now = dt.AddSeconds(step * i);
                load.Series.Points.AddXY(now, input[i]);
            }
        }

        public void LoadFrequencyChart(int number, List<ComplexNumber> input) {
            FrequencyChannel load;
            if (number == 2) {
                load = FrequencyChannel2;
            } else {
                load = FrequencyChannel1;
            }

            for (int i = 0; i < input.Count; i++) {
                load.Series.Points.AddXY(i, input[i].Length);
            }
        }

        public AudioPanelMenu Menu { get; set; }

        public TrackBar LevelSlide1 { get; set; }
        public TrackBar LevelSlide2 { get; set; }
        public AudioChannel TimeChannel1 { get; set; }
        public AudioChannel TimeChannel2 { get; set; }
        public FrequencyChannel FrequencyChannel1 { get; set; }
        public FrequencyChannel FrequencyChannel2 { get; set; }

        public ScrollableControl ParentRef { get; set; }
        public const int PADDING = 12;
        public const int MENU_HEIGHT = 24;
        public const int SLIDER_WIDTH = 60;
        public const int MONO_HEIGHT = 240;
        public const int STEREO_HEIGHT = MONO_HEIGHT + AudioChannel.HEIGHT;

        public AudioPanelData PanelData { get; set; }
    }
}
