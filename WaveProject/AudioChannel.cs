using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject {
    class AudioChannel {

        public AudioChannel() : this("Audio Channel") {
        }

        public AudioChannel(String name) : this(name, 0, 0) {
        }

        public AudioChannel(String name, int xPos, int yPos) : this(name, xPos, yPos, 100, 100) {
        }

        public AudioChannel(String name, int xPos, int yPos, int width, int height) {
            this.chart = new Chart();
            this.chartArea = new ChartArea();
            this.series = new Series();

            Init(name, xPos, yPos, width, height);
        }

        private void Init(String name, int xPos, int yPos, int width, int height) {
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            InitChartArea(name);
            InitSeries();
            InitChart(name, xPos, yPos, width, height);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
        }

        private void InitChartArea(String name) {
            chartArea.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Green;
            chartArea.AxisX.MajorGrid.LineWidth = 0;
            chartArea.AxisX.Minimum = 0D;
            chartArea.AxisY.LabelStyle.ForeColor = Color.Green;
            chartArea.AxisY.MajorGrid.LineWidth = 0;
            chartArea.AxisY.Title = name;
            chartArea.AxisY.TitleForeColor = Color.Red;
            chartArea.BackColor = Color.Black;
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.Name = "ChartArea";

            chart.ChartAreas.Add(chartArea);
        }

        private void InitSeries() {
            series.BorderWidth = 2;
            series.ChartArea = "ChartArea";
            series.ChartType = SeriesChartType.Spline;
            series.Color = Color.Green;
            series.Name = "Series";
            series.Points.AddXY(0, 120);
            series.Points.AddXY(1, 0);

            chart.Series.Add(series);
        }

        private void InitChart(String name, int xPos, int yPos, int width, int height) {
            chart.Size = new Size(width, height);
            chart.TabIndex = 1;
            chart.Text = name;
            chart.Location = new Point(xPos, yPos);
            chart.Name = name;
            chart.BackColor = Color.Black;
        }

        private Chart chart;
        private ChartArea chartArea;
        private Series series;

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
    }
}
