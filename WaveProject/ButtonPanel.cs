using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WaveProject {
    class ButtonPanel : Panel {

        public ButtonPanel(Point position, Size parentSize) {
            this.play = new CircleButton();
            this.Controls.Add(this.play);
            Init(position, parentSize);
        }

        private void Init(Point position, Size parentSize) {
            this.Location = position;
            this.ClientSize = new Size(parentSize.Width, 77);
            this.BackColor = Color.FromArgb(255, 35, 35, 35);

            play.Location = new Point((this.ClientSize.Width / 2) - 33, 5);
            play.Image = new Bitmap("C:\\Users\\Xeltide\\Desktop\\Archive\\BCIT\\Term 3\\COMP 3931\\Project\\WaveProject\\WaveProject\\Play.png");
            play.BackColor = Color.FromArgb(255, 17, 17, 17);
            play.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            play.FlatAppearance.BorderSize = 0;
            play.Name = "PlayButton";
            play.Size = new Size(66, 66);
            play.TabIndex = 1;
            play.UseVisualStyleBackColor = true;
        }

        public void ResizePanel(Size parentSize) {
            this.ClientSize = new Size(parentSize.Width, 77);
            play.Location = new Point((this.ClientSize.Width / 2) - 33, 5);
        }

        private CircleButton play;
    }
}
