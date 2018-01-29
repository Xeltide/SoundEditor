using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace WaveProject
{
    class AP_PlayBar : Panel
    {

        public AP_PlayBar(AudioPanel panel, int xPos, int yPos, int width, int height)
        {
            this.panel = panel;
            this.BackColor = Color.FromArgb(255, 40, 40, 40);
            this.Location = new Point(xPos, yPos);
            this.ClientSize = new Size(width, height);

            play = new Button();
            play.Text = "Play";
            play.ForeColor = Color.White;
            play.Click += new EventHandler(Play_Click);
            play.Location = new Point(5, 5);
            this.Controls.Add(play);

            stop = new Button();
            stop.Text = "Stop";
            stop.ForeColor = Color.White;
            stop.Click += new EventHandler(Stop_Click);
            stop.Location = new Point(105, 5);
            stop.Enabled = false;
            this.Controls.Add(stop);

            record = new Button();
            record.Text = "Record";
            record.ForeColor = Color.White;
            record.Click += new EventHandler(Record_Click);
            record.Location = new Point(210, 5);
            this.Controls.Add(record);

            timer = new System.Timers.Timer(100.0);
            timer.Elapsed += HandleTimer;
            timeElapsed = 0;

            time = new Label();
            time.Text = 0 + " / " + 0 + " seconds";
            time.ForeColor = Color.White;
            time.Width = 200;
            time.Location = new Point(5, 35);
            this.Controls.Add(time);
        }

        public void Enable_Record(bool enabled)
        {
            if (!enabled)
            {
                this.record.Text = "Record";
            }
            else
            {
                this.record.Text = "Stop";
            }
            this.record.Enabled = enabled;
        }

        private void HandleTimer(Object sender, EventArgs e)
        {
            timeElapsed += 0.1;
            double timeMax = ((int)(((panel.PanelData.RIFFData.Data.Length / (double)(panel.PanelData.RIFFData.SampleRate * (panel.PanelData.RIFFData.BitsPerSample / 8))) / panel.PanelData.RIFFData.Channels) * 10)) / 10.0;
            if (timeElapsed < timeMax)
            {
                Invoke(new MethodInvoker(() =>
                {
                    time.Text = Math.Round(timeElapsed * 10) / 10.0 + " / " + timeMax + " seconds";
                }));
            } else
            {
                Invoke(new MethodInvoker(() =>
                {
                    play.Text = "Play";
                    stop.Enabled = false;
                    time.Text = timeMax + " / " + timeMax + " seconds";
                }));
                timer.Enabled = false;
                timeElapsed = timeMax;
            }
        }

        private void Play_Click(object sender, EventArgs e)
        {
            if (!Service.DLLWrapper.IsPlayingDLL() && panel.PanelData.RIFFData.SubChunkSize > 0)
            {
                timeElapsed = 0;
                Service.DLLWrapper.SetPlayBuffer(panel.PanelData.RIFFData);
                Service.DLLWrapper.Play();
                timer.Enabled = true;
                this.play.Text = "Pause";
                this.stop.Enabled = true;
            }
            else if (Service.DLLWrapper.IsPlayingDLL())
            {
                if (Service.DLLWrapper.IsPausedDLL())
                {
                    timer.Enabled = true;
                    this.play.Text = "Pause";
                }
                else
                {
                    timer.Enabled = false;
                    this.play.Text = "Play";
                }
                Service.DLLWrapper.Pause();
            }
        }

        public void Stop_Click(object sender, EventArgs e)
        {
            if (Service.DLLWrapper.IsPlayingDLL())
            {
                timer.Enabled = false;
                timeElapsed = 0;
                play.Text = "Play";
                stop.Enabled = false;
                time.Text = 0 + " / " + 0 + " seconds";
                Service.DLLWrapper.Stop();
            }
        }

        public void Record_Click(object sender, EventArgs e)
        {
            if (Service.DLLWrapper.Safe)
            {
                if (panel.PanelData.RIFFData.SubChunkSize == 0)
                {
                    recording = !recording;
                    if (recording)
                    {
                        play.Enabled = false;
                        record.Text = "Stop";
                        WavPackager packager = new WavPackager(null);
                        RIFFData riff = packager.GenerateRIFFData(panel);
                        panel.PanelData.RIFFData = riff;
                        Service.DLLWrapper.Record(riff);
                    }
                    else
                    {
                        play.Enabled = true;
                        Enable_Record(false);
                        RIFFData riff = panel.PanelData.RIFFData;
                        riff.Data = Service.DLLWrapper.RecordStop();
                        riff.SubChunkSize = riff.Data.Length;
                        panel.PanelData.RIFFData = riff;

                        panel.PanelData.SplitWavData();
                        panel.ClearAllCharts();

                        panel.PanelData.RawChartData.Add(panel.PanelData.GetReadableChannel(0));

                        Workers.ChartRenderWorker worker = new Workers.ChartRenderWorker(panel, 0, true);
                        worker.RunWorkerAsync();

                        if (panel.PanelData.RawChannelData.Count > 1)
                        {
                            panel.Menu.manager.Stereo_Click();
                            panel.Menu.manager.Mono.Enabled = false;

                            panel.PanelData.RawChartData.Add(panel.PanelData.GetReadableChannel(1));

                            Workers.ChartRenderWorker worker2 = new Workers.ChartRenderWorker(panel, 1, true);
                            worker2.RunWorkerAsync();
                        }
                        else
                        {
                            panel.Menu.manager.Mono_Click();
                            panel.Menu.manager.Stereo.Enabled = false;
                        }
                    }

                    panel.Menu.manager.Open_Click();
                }
                else
                {
                    ScrollPanel scroll = (ScrollPanel)panel.ParentRef;
                    scroll.Click_AddChannel(null, EventArgs.Empty);
                    scroll.audioPanels.Last.Value.PlayBar.Record_Click(null, EventArgs.Empty);
                }
            }
            else
            {
                Console.WriteLine("Unsafe Record action attempted and ignored.");
            }
        }

        private bool recording = false;
        private AudioPanel panel;
        private Button play;
        private Button stop;
        private Button record;
        private Label time;
        private double timeElapsed;

        System.Timers.Timer timer;
    }
}
