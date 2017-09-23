using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WaveProject {
    class ChannelPanel : Panel {

        public ChannelPanel(Point position, Size parentSize) {
            mstrChannel = new MasterChannel();
            Init(position, parentSize);
        }

        private void Init(Point position, Size parentSize) {
            this.Location = position;
            this.ClientSize = new Size(parentSize.Width - 24, parentSize.Height - position.Y - 4);
            this.MaximumSize = new Size(1920, 420);

            AudioChannel acl = new AudioChannel("L Channel", 0, 0, this.ClientSize.Width, this.ClientSize.Height / 2);
            AudioChannel acr = new AudioChannel("R Channel", 0, this.ClientSize.Height / 2, this.ClientSize.Width, this.ClientSize.Height / 2);
            acr.ChartArea.AxisY.TitleForeColor = Color.CornflowerBlue;
            mstrChannel.AddChannel(acl);
            mstrChannel.AddChannel(acr);

            foreach (AudioChannel a in mstrChannel.Channels) {
                this.Controls.Add(a.Chart);
            }
        }

        // FUNCTIONS START
        public void Resize_Panel(Size parentSize) {
            this.ClientSize = new Size(parentSize.Width - 24, parentSize.Height - Location.Y);
            mstrChannel.Channels.ElementAt(0).Chart.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height / 2);
            if (this.ClientSize.Height <= 420) {
                mstrChannel.Channels.ElementAt(1).Chart.Location = new Point(0, this.ClientSize.Height / 2);
            }
            mstrChannel.Channels.ElementAt(1).Chart.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height / 2);
        }
        // FUNCTIONS END

        // PROPERTIES START
        public MasterChannel MasterChannel { get; set; }
        // PROPERTIES END

        private MasterChannel mstrChannel;
    }
}
