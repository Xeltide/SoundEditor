using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace WaveProject {
    class AudioChannel {
        
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
        public void Resize_Chart(Size parentSize) {
            this.chart.ClientSize = parentSize;
        }

        protected void MouseEnter_Chart(object sender, EventArgs e) {
            this.Chart.Focus();
        }

        protected void MouseExit_Chart(object sender, EventArgs e) {
            Window.window.Focus();
        }

        protected void Zoom_Chart(object sender, System.Windows.Forms.MouseEventArgs e) {
            if (e.Delta < 0) {
                ChartArea.AxisX.ScaleView.ZoomReset();
            }

            if (e.Delta > 0) {
                double xMin = ChartArea.AxisX.ScaleView.ViewMinimum;
                double xMax = ChartArea.AxisX.ScaleView.ViewMaximum;

                // Internal function call of PixelPositionToValue fails sometimes
                try
                {
                    double posXStart = ChartArea.AxisX.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                    double posXFinish = ChartArea.AxisX.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;

                    if ((int)posXFinish - (int)posXStart > 1)
                    {
                        ChartArea.AxisX.ScaleView.Zoom(posXStart, posXFinish);
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                
            }

            RefreshViewablePoints();
        }

        protected void Scroll_Chart(object sender, EventArgs e) {
            RefreshViewablePoints();
        }

        public void RefreshViewablePoints() {
            if (worker == null) {
                if (this.Equals(Panel.TimeChannel1)) {
                    worker = new Workers.ChartRenderWorker(Panel, 0, false);
                } else {
                    worker = new Workers.ChartRenderWorker(Panel, 1, false);
                }
            } else if (worker.IsBusy) {
                try {
                    worker.CancelAsync();
                } catch { /* Race condition */ };
            }

            try {
                worker.RunWorkerAsync();
            } catch { /* Race condition */ };
        }
        // FUNCTIONS END

        // INIT START
        private void Init(String name, int xPos, int yPos, int width, int height) {
            InitChartArea(name);
            InitSeries();
            InitChart(name, xPos, yPos, width, height);
            
            chart.MouseEnter += new EventHandler(MouseEnter_Chart);
            chart.MouseLeave += new EventHandler(MouseExit_Chart);
            chart.MouseWheel += new System.Windows.Forms.MouseEventHandler(Zoom_Chart);
            chart.AxisViewChanged += new EventHandler<ViewEventArgs>(Scroll_Chart);
        }

        private void InitChartArea(String name) {
            chartArea.BackColor = Color.FromArgb(255, 50, 50, 50);
            chartArea.Name = "ChartArea";

            chartArea.AxisX.Title = name;
            chartArea.AxisX.TitleForeColor = Color.LightGreen;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisX.MajorGrid.LineColor = Color.DimGray;
            chartArea.AxisX.Minimum = 0D;
            chartArea.AxisX.ScaleView.Zoomable = false;
            chartArea.AxisX.IsMarginVisible = false;
            chartArea.AxisX.IntervalType = DateTimeIntervalType.Number;
            chartArea.AxisX.LabelStyle.ForeColor = Color.LightGreen;
            chartArea.AxisX.LabelStyle.Format = "#";

            chartArea.AxisY.LabelStyle.ForeColor = Color.LightGreen;
            chartArea.AxisY.MajorGrid.LineWidth = 0;
            chartArea.AxisY.LineColor = Color.DimGray;

            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.Position = 0;
            chartArea.CursorX.IntervalType = DateTimeIntervalType.Number;
            chartArea.CursorX.Interval = 0;

            chart.ChartAreas.Add(chartArea);
        }

        private void InitSeries() {
            series.BorderWidth = 1;
            series.ChartArea = "ChartArea";
            series.ChartType = SeriesChartType.FastLine;
            series.Color = Color.Green;
            series.Name = "Series";
            
            chart.Series.Add(series);
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
        private Workers.ChartRenderWorker worker;
        public AudioPanel Panel { get; set; }
        public const int HEIGHT = 210;
        // PROPERTIES END
    }
}
