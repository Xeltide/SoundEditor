using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject {
    class FrequencyChannel {
        private MasterChannel master;
        private Chart chart;
        private ChartArea chartArea;
        private Series series;
        public bool triggeredSelectionReset = false;

        // CONSTRUCTORS START
        public FrequencyChannel() : this("Frequency Channel") { }

        public FrequencyChannel(String name) : this(name, 0, 0) { }

        public FrequencyChannel(String name, int xPos, int yPos) : this(name, xPos, yPos, 100, 100) { }

        public FrequencyChannel(String name, int xPos, int yPos, int width, int height) {
            this.chart = new Chart();
            this.chartArea = new ChartArea();
            this.series = new Series();

            Init(name, xPos, yPos, width, height);
        }
        // CONSTRUCTORS END

        // INIT START
        private void Init(String name, int xPos, int yPos, int width, int height) {
            InitChartArea(name);
            InitSeries();
            InitChart(name, xPos, yPos, width, height);

            //chart.CursorPositionChanging += new EventHandler<CursorEventArgs>(CursorXChanged);
            //chart.SelectionRangeChanging += new EventHandler<CursorEventArgs>(SelectionXChanged);
        }

        private void InitChartArea(String name) {
            chartArea.AxisX.LabelStyle.ForeColor = Color.LightGreen;
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisX.Minimum = 0D;
            chartArea.AxisY.LabelStyle.ForeColor = Color.LightGreen;
            chartArea.AxisY.MajorGrid.LineWidth = 0;
            chartArea.AxisY.Title = name;
            chartArea.AxisY.TitleForeColor = Color.Red;
            //SET THE ZOOM LEVEL VIA FUNCTION DURING MOUSEWHEEL EVENT
            chartArea.BackColor = Color.FromArgb(255, 35, 35, 35);
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.AxisX.ScaleView.Zoomable = false;
            chartArea.CursorX.Position = 0;
            //chartArea.CursorX.IntervalType = DateTimeIntervalType.Seconds;

            //chartArea.AxisX.IntervalType = DateTimeIntervalType.Seconds;
            //chartArea.AxisX.Interval = 1;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Seconds;
            //chartArea.AxisX.LabelStyle.Format = "mm:ss";
            chartArea.Name = "ChartArea";

            chart.ChartAreas.Add(chartArea);
        }

        private void InitSeries() {
            //series.XValueType = ChartValueType.Time;
            series.BorderWidth = 2;
            series.ChartArea = "ChartArea";
            //series.ChartType = SeriesChartType.Spline;
            series.Color = Color.Green;
            series.Name = "Series";

            // Dummy data set
            DateTime dt = DateTime.Today;
            for (int i = 0; i < 10; i++) {
                DateTime cur = dt.AddSeconds(i);
                int y;
                if (i % 2 == 0) {
                    y = 140;
                } else if (i % 3 == 0) {
                    y = -140;
                } else {
                    y = 0;
                }
                series.Points.AddXY((DateTime)cur, y);
            }

            chart.Series.Add(series);
        }

        private void InitChart(String name, int xPos, int yPos, int width, int height) {
            chart.Size = new Size(width, height);
            chart.TabIndex = 1;
            chart.Text = name;
            chart.Location = new Point(xPos, yPos);
            chart.Name = name;
            chart.BackColor = Color.FromArgb(255, 25, 25, 25);
        }
        // INIT END

        // PROPERTIES START
        public Chart Chart { get; set; }
        public Series Series { get; set; }
        public ChartArea ChartArea { get; set; }
        public MasterChannel MasterChannel { get; set; }
        // PROPERTIES END
    }
}
