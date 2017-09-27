using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WaveProject {
    class ScrollPanel : Panel {

        public ScrollPanel(ScrollableControl parent) {
            this.ParentRef = parent;
            this.VirtualHeight = ADD_BUTTON_TOTALH;
            this.Location = new Point(0, Window.HEADER_HEIGHT);
            this.ClientSize = new Size(parent.ClientSize.Width, VirtualHeight);
            audioPanels = new LinkedList<AudioPanel>();
            Init_AddChannel();
            Resize_Panel();
        }
        private void Init_AddChannel() {
            addChannelButton = new CircleButton();

            addChannelButton.Size = new Size(ADD_BUTTON_SIZE, ADD_BUTTON_SIZE);
            addChannelButton.Location = new Point((this.ClientSize.Width / 2) - addChannelButton.Size.Width / 2, ADD_BUTTON_PADDING);
            addChannelButton.Image = new Bitmap("C:\\Users\\Xeltide\\Desktop\\Archive\\BCIT\\Term 3\\COMP 3931\\Project\\WaveProject\\WaveProject\\AddImage.png");
            addChannelButton.BackColor = Color.FromArgb(255, 30, 30, 30);
            addChannelButton.FlatStyle = FlatStyle.Flat;
            addChannelButton.FlatAppearance.BorderSize = 0;
            addChannelButton.Name = "AddChannelButton";
            addChannelButton.TabIndex = 1;
            addChannelButton.UseVisualStyleBackColor = true;
            addChannelButton.Click += new EventHandler(Click_AddChannel);

            this.Controls.Add(addChannelButton);
        }

        private void Click_AddChannel(object sender, EventArgs e) {
            Point p = new Point(0, SumChannelHeight());
            AudioPanel addedPanel = new AudioPanel(this, p);

            audioPanels.AddLast(addedPanel);
            this.Controls.Add(addedPanel);
            
            Resize_Panel();
        }

        // Includes the menu bar, button panel, and virtual space
        private int SumChannelHeight() {
            int sum = 0;
            foreach (AudioPanel ap in audioPanels) {
                if (!ap.Menu.IsMono) {
                    sum += AudioPanel.STEREO_HEIGHT;
                } else {
                    sum += AudioPanel.MONO_HEIGHT;
                }
            }
            return sum;
        }

        public void Resize_Panel() {
            
            //Resize and position AudioPanels
            Resize_AudioPanels();

            Adjust_ScrollBar();

            // Resize this scroll panel
            VScrollBar scroll = ((Window)ParentRef).ScrollBar;
            if (scroll.Visible) {
                this.ClientSize = new Size(ParentRef.ClientSize.Width - scroll.Width, VirtualHeight);
            } else {
                this.ClientSize = new Size(ParentRef.ClientSize.Width, VirtualHeight);
            }

            if (scrollToggled) {
                Resize_AudioPanels();
                scrollToggled = false;
            } else if (ResizeEvent) {
                Resize_AudioPanels();
                ResizeEvent = false;
            }

            // Reposition the add channel button
            addChannelButton.Location = new Point((this.ClientSize.Width / 2) - addChannelButton.Width / 2, SumChannelHeight() + ADD_BUTTON_PADDING);
        }

        private void Adjust_ScrollBar() {
            int viewHeight = ((Window)ParentRef).ShowScrollHeight;
            VScrollBar scroll = ((Window)ParentRef).ScrollBar;
            if (VirtualHeight > viewHeight) {
                scroll.LargeChange = viewHeight / 4;
                scroll.Maximum = (VirtualHeight - viewHeight) / Window.SCROLL_FACTOR;
                scroll.Maximum += scroll.LargeChange;
                if (!scroll.Visible) {
                    scrollToggled = true;
                    scroll.Show();
                }
            } else if (scroll.Visible) {
                scrollToggled = true;
                scroll.Hide();
            }
        }

        private void Resize_AudioPanels() {
            int yPos = 0;
            foreach (AudioPanel ap in audioPanels) {
                ap.Location = new Point(AudioPanel.PADDING, yPos);
                ap.Resize_Panel();
                if (ap.Menu.IsMono) {
                    yPos += AudioPanel.MONO_HEIGHT;
                } else {
                    yPos += AudioPanel.STEREO_HEIGHT;
                }
            }
            VirtualHeight = yPos + ADD_BUTTON_TOTALH;
        }

        public bool ResizeEvent { get; set; }
        public int VirtualHeight { get; set; }
        private bool scrollToggled = false;
        private CircleButton addChannelButton;
        private LinkedList<AudioPanel> audioPanels;
        public ScrollableControl ParentRef { get; set; }
        public const int ADD_BUTTON_PADDING = 40;
        public const int ADD_BUTTON_SIZE = 122;
        public const int ADD_BUTTON_TOTALH = (2 * ADD_BUTTON_PADDING) + ADD_BUTTON_SIZE;
    }
}
