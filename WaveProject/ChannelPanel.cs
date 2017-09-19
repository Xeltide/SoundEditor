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

            AudioChannel acl = new AudioChannel("L Channel", 0, 0, this.ClientSize.Width, this.ClientSize.Height / 2);
            AudioChannel acr = new AudioChannel("R Channel", 0, this.ClientSize.Height / 2, this.ClientSize.Width, this.ClientSize.Height / 2);
            mstrChannel.AddChannel(acl);
            mstrChannel.AddChannel(acr);

            foreach (AudioChannel a in mstrChannel.Channels) {
                this.Controls.Add(a.Chart);
            }
        }

        // FUNCTIONS START
        public void ResizePanel(Size parentSize) {
            this.ClientSize = new Size(parentSize.Width - 24, parentSize.Height - Location.Y - 4);
            mstrChannel.Channels.ElementAt(0).Chart.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height / 2);
            mstrChannel.Channels.ElementAt(1).Chart.Location = new Point(0, this.ClientSize.Height / 2);
            mstrChannel.Channels.ElementAt(1).Chart.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height / 2);
        }
        // FUNCTIONS END

        // PROPERTIES START
        public MasterChannel MasterChannel { get; set; }
        // PROPERTIES END

        private MasterChannel mstrChannel;
    }
}
