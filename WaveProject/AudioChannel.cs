using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject {
    class AudioChannel {

        private MasterChannel master;
        private Chart chart;
        private ChartArea chartArea;
        private Series series;
        public bool triggeredSelectionReset = false;

        // CONSTRUCTORS START
        public AudioChannel() : this("Audio Channel") {}

        public AudioChannel(String name) : this(name, 0, 0) {}

        public AudioChannel(String name, int xPos, int yPos) : this(name, xPos, yPos, 100, 100) {}

        public AudioChannel(String name, int xPos, int yPos, int width, int height) {
            this.chart = new Chart();
            this.chartArea = new ChartArea();
            this.series = new Series();

            Init(name, xPos, yPos, width, height);
        }
        // CONSTRUCTORS END

        // FUNCTIONS START
        protected void CursorXChanged(object sender, CursorEventArgs e) {
            if (master != null) {
                master.CursorX = e.NewPosition;
            }
        }

        protected void SelectionXChanged(object sender, CursorEventArgs e) {
            if (master != null) {
                triggeredSelectionReset= true;
                master.ClearChannelSelection();
                triggeredSelectionReset = false;
            }
        }

        public void Resize_Chart(Size parentSize) {
            this.chart.ClientSize = parentSize;
        }
        // FUNCTIONS END

        // INIT START
        private void Init(String name, int xPos, int yPos, int width, int height) {
            InitChartArea(name);
            InitSeries();
            InitChart(name, xPos, yPos, width, height);
            
            chart.CursorPositionChanging += new EventHandler<CursorEventArgs>(CursorXChanged);
            chart.SelectionRangeChanging += new EventHandler<CursorEventArgs>(SelectionXChanged);
        }

        private void InitChartArea(String name) {
            chartArea.BackColor = Color.FromArgb(255, 50, 50, 50);
            chartArea.Name = "ChartArea";

            chartArea.AxisX.Title = name;
            chartArea.AxisX.TitleForeColor = Color.LightGreen;
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisX.MajorGrid.LineColor = Color.DimGray;
            chartArea.AxisX.Minimum = 0D;
            chartArea.AxisX.ScaleView.Zoomable = true;
            // DateTime.AddSeconds is being converted to Days?
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Seconds;
            // Interval = sample rate gives seconds lines
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Interval = 1;
            chartArea.AxisX.LabelStyle.ForeColor = Color.LightGreen;
            chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            chartArea.AxisX.LabelStyle.Format = "mm:ss";

            chartArea.AxisY.LabelStyle.ForeColor = Color.LightGreen;
            chartArea.AxisY.MajorGrid.LineWidth = 0;
            chartArea.AxisY.LineColor = Color.DimGray;

            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.Position = 0;
            chartArea.CursorX.IntervalType = DateTimeIntervalType.Milliseconds;
            //SET THE ZOOM LEVEL VIA FUNCTION DURING MOUSEWHEEL EVENT
            
            chart.ChartAreas.Add(chartArea);
        }

        private void InitSeries() {
            series.XValueType = ChartValueType.Time;
            series.BorderWidth = 1;
            series.ChartArea = "ChartArea";
            series.ChartType = SeriesChartType.FastLine;
            series.Color = Color.Green;
            series.Name = "Series";
            
            chart.Series.Add(series);
        }

        private void GenerateComplexWave(Series series) {            
            int Nsamples = 1000;
            int f = 6; // revolutions/sec
            int A = 3;
            double step = 1.0 / Nsamples;
            DateTime dt = DateTime.Today;
            for (int time = 0; time < Nsamples; time++) {
                DateTime cur = dt.AddSeconds(step * time);
                double point = A * Math.Cos(2 * Math.PI * f * time / Nsamples);
                series.Points.AddXY(dt, point);
            }
        }

        private void InitChart(String name, int xPos, int yPos, int width, int height) {
            chart.ClientSize = new Size(width, height);
            chart.TabIndex = 1;
            chart.Text = name;
            chart.Location = new Point(xPos, yPos);
            chart.Name = name;
            chart.BackColor = Color.FromArgb(255, 25, 25, 25);
            chart.MaximumSize = new Size(1920, 210);
            chart.MinimumSize = new Size(0, 210);
        }
        
        // PROPERTIES START
        public Chart Chart {
            get {
                return chart;
            }
            set {
                chart = value;
            }
        }
        public Series Series {
            get {
                return series;
            }
            set {
                series = value;
            }
        }
        public ChartArea ChartArea {
            get {
                return chartArea;
            }
            set {
                chartArea = value;
            }
        }
        public MasterChannel MasterChannel {
            get {
                return master;
            }
            set {
                master = value;
            }
        }
        public const int HEIGHT = 210;
        // PROPERTIES END
    }
}
