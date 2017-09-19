using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WaveProject {
    class MasterChannel : INotifyPropertyChanged {

        private LinkedList<Byte> masterData;
        private LinkedList<AudioChannel> channels;
        private double cursorX = 0;
        
        public MasterChannel() {
            masterData = new LinkedList<Byte>();
            channels = new LinkedList<AudioChannel>();
            this.PropertyChanged += new PropertyChangedEventHandler(CursorXChanged);
        }

        public void AddChannel(AudioChannel channel) {
            channels.AddLast(channel);
            channel.MasterChannel = this;
        }

        public void ClearChannelSelection() {
            foreach (AudioChannel a in channels) {
                if (!a.triggeredSelectionReset) {
                    a.ChartArea.CursorX.SetSelectionPosition(0, 0);
                }
            }
        }

        // PROPERTIES START
        public double CursorX {
            get {
                return cursorX;
            }
            set {
                if (value != cursorX) {
                    cursorX = value;
                    OnPropertyChanged("CursorX");
                }
            }
        }

        public LinkedList<AudioChannel> Channels {
            get {
                return channels;
            }
            set {
                channels = value;
            }
        }
        // PROPERTIES END

        // PROPERTY CHANGE START
        protected void CursorXChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName.Equals("CursorX")) {
                foreach (AudioChannel a in channels) {
                    a.ChartArea.CursorX.Position = cursorX;
                }
            }
        }

        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        // PROPERTY CHANGE END

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
