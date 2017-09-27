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

        public void Resize_Chart(Point position, Size parentSize) {
            chart.Location = position;
            chart.ClientSize = parentSize;
        }

        // INIT START
        private void Init(String name, int xPos, int yPos, int width, int height) {
            InitChartArea(name);
            InitSeries();
            InitChart(name, xPos, yPos, width, height);

            //chart.CursorPositionChanging += new EventHandler<CursorEventArgs>(CursorXChanged);
            //chart.SelectionRangeChanging += new EventHandler<CursorEventArgs>(SelectionXChanged);
        }

        private void InitChartArea(String name) {
            chartArea.BackColor = Color.FromArgb(255, 50, 50, 50);

            chartArea.AxisX.LabelStyle.ForeColor = Color.LightGreen;
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisX.Minimum = 0D;
            chartArea.AxisY.Minimum = 0D;
            chartArea.AxisY.LabelStyle.ForeColor = Color.LightGreen;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            chartArea.AxisX.Title = name;
            chartArea.AxisY.TitleForeColor = Color.Red;
            //SET THE ZOOM LEVEL VIA FUNCTION DURING MOUSEWHEEL EVENT
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.CursorX.Position = 0;
            chartArea.Name = "ChartArea";

            chart.ChartAreas.Add(chartArea);
        }

        private void InitSeries() {
            //series.XValueType = ChartValueType.Auto;
            series.BorderWidth = 2;
            series.ChartArea = "ChartArea";
            //series.ChartType = SeriesChartType.Spline;
            series.Color = Color.Green;
            series.Name = "Series";

            // Dummy data set
            /*for (int i = 0; i < 10; i++) {
                int y = i * 20;
                series.Points.AddXY(i, y);
            }*/

            chart.Series.Add(series);
        }

        private void InitChart(String name, int xPos, int yPos, int width, int height) {
            chart.Size = new Size(width, height);
            chart.TabIndex = 1;
            chart.Text = name;
            chart.Location = new Point(xPos, yPos);
            chart.Name = name;
            chart.BackColor = Color.FromArgb(255, 25, 25, 25);
            chart.MinimumSize = new Size(0, 210);
            chart.MaximumSize = new Size(1920, 210);
        }
        // INIT END

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
        public const int SIZE = 210;
        public const int WIDTH_PADDING = 48;
        // PROPERTIES END
    }
}
